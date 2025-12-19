using STranslate.Plugin.Translate.Volcengine.View;
using STranslate.Plugin.Translate.Volcengine.ViewModel;
using System.Text;
using System.Text.Json.Nodes;
using System.Windows.Controls;

namespace STranslate.Plugin.Translate.Volcengine;

public class Main : LlmTranslatePluginBase
{
    private Control? _settingUi;
    private SettingsViewModel? _viewModel;
    private Settings Settings { get; set; } = null!;
    private IPluginContext Context { get; set; } = null!;

    private readonly int[] _config = [90, 109, 69, 52, 77, 109, 77, 122, 79, 84, 99, 119, 78, 68, 85, 122, 78, 87, 69, 52, 78, 106, 107, 50, 77, 87, 78, 105, 77, 109, 90, 108, 79, 87, 81, 48, 79, 84, 107, 53, 77, 106, 103, 117, 87, 110, 74, 90, 87, 109, 120, 82, 90, 87, 49, 71, 99, 69, 48, 121, 86, 88, 82, 104, 87, 81, 61, 61];
    internal string GetFallbackKey() => string.Concat(_config.Select(x => (char)x));

    public override void SelectPrompt(Prompt? prompt)
    {
        base.SelectPrompt(prompt);

        // 保存到配置
        Settings.Prompts = [.. Prompts.Select(p => p.Clone())];
        Context.SaveSettingStorage<Settings>();
    }

    public override Control GetSettingUI()
    {
        _viewModel ??= new SettingsViewModel(Context, Settings, this);
        _settingUi ??= new SettingsView { DataContext = _viewModel };
        return _settingUi;
    }

    public override string? GetSourceLanguage(LangEnum langEnum) => langEnum switch
    {
        LangEnum.Auto => "Requires you to identify automatically",
        LangEnum.ChineseSimplified => "zh",
        LangEnum.ChineseTraditional => "zh-Hant",
        LangEnum.English => "en",
        LangEnum.Japanese => "ja",
        LangEnum.Korean => "ko",
        LangEnum.German => "de",
        LangEnum.French => "fr",
        LangEnum.Spanish => "es",
        LangEnum.Italian => "it",
        LangEnum.PortuguesePortugal => "pt",
        LangEnum.PortugueseBrazil => "pt",
        LangEnum.Russian => "ru",
        LangEnum.Thai => "th",
        LangEnum.Vietnamese => "vi",
        LangEnum.Arabic => "ar",
        LangEnum.Indonesian => "id",
        LangEnum.Malay => "ms",
        LangEnum.NorwegianBokmal => "nb",
        LangEnum.Dutch => "nl",
        LangEnum.Polish => "pl",
        LangEnum.Swedish => "sv",
        LangEnum.Turkish => "tr",
        LangEnum.Ukrainian => "uk",
        _ => "Requires you to identify automatically"
    };

    public override string? GetTargetLanguage(LangEnum langEnum) => langEnum switch
    {
        LangEnum.Auto => "Requires you to identify automatically",
        LangEnum.ChineseSimplified => "zh",
        LangEnum.ChineseTraditional => "zh-Hant",
        LangEnum.English => "en",
        LangEnum.Japanese => "ja",
        LangEnum.Korean => "ko",
        LangEnum.German => "de",
        LangEnum.French => "fr",
        LangEnum.Spanish => "es",
        LangEnum.Italian => "it",
        LangEnum.PortuguesePortugal => "pt",
        LangEnum.PortugueseBrazil => "pt",
        LangEnum.Russian => "ru",
        LangEnum.Thai => "th",
        LangEnum.Vietnamese => "vi",
        LangEnum.Arabic => "ar",
        LangEnum.Indonesian => "id",
        LangEnum.Malay => "ms",
        LangEnum.NorwegianBokmal => "nb",
        LangEnum.Dutch => "nl",
        LangEnum.Polish => "pl",
        LangEnum.Swedish => "sv",
        LangEnum.Turkish => "tr",
        LangEnum.Ukrainian => "uk",
        _ => "Requires you to identify automatically"
    };

    public override void Init(IPluginContext context)
    {
        Context = context;
        Settings = context.LoadSettingStorage<Settings>();

        Settings.Prompts.ForEach(Prompts.Add);
    }

    public override void Dispose() => _viewModel?.Dispose();

    public override async Task TranslateAsync(TranslateRequest request, TranslateResult result, CancellationToken cancellationToken = default)
    {
        if (GetSourceLanguage(request.SourceLang) is not string sourceStr)
        {
            result.Fail(Context.GetTranslation("UnsupportedSourceLang"));
            return;
        }
        if (GetTargetLanguage(request.TargetLang) is not string targetStr)
        {
            result.Fail(Context.GetTranslation("UnsupportedTargetLang"));
            return;
        }


        UriBuilder uriBuilder = new(Settings.Url);
        // 如果路径不是有效的API路径结尾，使用默认 Ark 翻译路径
        if (uriBuilder.Path == "/")
            uriBuilder.Path = "/api/v3/responses";

        // 选择模型
        var model = Settings.Model.Trim();
        model = string.IsNullOrEmpty(model) ? "doubao-seed-translation-250915" : model;

        // 替换 Prompt 关键字并合并为单条输入文本（用于 Ark 翻译模型）
        var prompt = (Prompts.FirstOrDefault(x => x.IsEnabled) ?? throw new Exception("请先完善Prompt配置")).Clone();
        var items = prompt.Items;
        items.ToList().ForEach(item =>
            item.Content = item.Content
                .Replace("$source", sourceStr)
                .Replace("$target", targetStr)
                .Replace("$content", request.Text)
        );

        // 合并为单个文本段落，保留行间分隔
        var combinedText = string.Join("\n", items.Select(i => i.Content));

        // 温度限定（Ark 范围为 [0,2]，默认 1）
        var temperature = Math.Clamp(Settings.Temperature, 0, 2);

        // 构建 translation_options，source_language 可选
        object translationOptions = sourceStr == "Requires you to identify automatically"
            ? new { target_language = targetStr }
            : new { source_language = sourceStr, target_language = targetStr };

        var content = new
        {
            model,
            input = new[]
            {
                new
                {
                    role = "user",
                    content = new object[]
                    {
                        new
                        {
                            type = "input_text",
                            text = combinedText,
                            translation_options = translationOptions
                        }
                    }
                }
            },
            temperature,
            stream = true,
            thinking = new
            {
                type = Settings.Thinking ? "enabled" : "disabled"
            }
        };

        var option = new Options
        {
            Headers = new Dictionary<string, string>
            {
                { "authorization", "Bearer " + (Settings?.ApiKey ?? string.Empty) }
            }
        };

        await Context.HttpService.StreamPostAsync(uriBuilder.Uri.ToString(), content, msg =>
        {
            if (string.IsNullOrEmpty(msg?.Trim()))
                return;

            var preprocessString = msg.Replace("data:", "").Trim();

            // SSE 结束标记检查：兼容旧版 [DONE] 以及 Ark 的 response.completed 事件
            if (preprocessString.Equals("[DONE]"))
                return;

            if (msg.IndexOf("event: response.completed", StringComparison.OrdinalIgnoreCase) >= 0)
                return;

            try
            {
                /**
                 * 
                 * var parsedData = JsonDocument.Parse(preprocessString);

                if (parsedData is null)
                    return;

                var root = parsedData.RootElement;

                // 提取 content 的值
                var contentValue = root
                    .GetProperty("choices")[0]
                    .GetProperty("delta")
                    .GetProperty("content")
                    .GetString();
                * 
                 */
                // 解析JSON数据（兼容 Ark 返回格式与旧的 choices.delta.content）
                var parsedData = JsonNode.Parse(preprocessString);

                if (parsedData is null)
                    return;

                // 如果解析得到的对象本身表示一个 response.completed 事件（某些实现可能把 type 放在 data 中）
                var parsedType = parsedData["type"]?.ToString();
                if (!string.IsNullOrEmpty(parsedType) && parsedType.Equals("response.completed", StringComparison.OrdinalIgnoreCase))
                    return;

                // 首先尝试按 Ark 的返回格式解析：顶层 output 数组
                if (parsedData["output"] is JsonArray outputArray)
                {
                    foreach (var outItem in outputArray)
                    {
                        if (outItem is null)
                            continue;

                        var outType = outItem["type"]?.ToString();

                        // 跳过推理/思考内容（在 Ark 示例中为 type == "reasoning" 并包含 summary）
                        if (string.Equals(outType, "reasoning", StringComparison.OrdinalIgnoreCase))
                            continue;

                        // message / assistant 类型的输出，content 为数组，提取 output_text
                        if (string.Equals(outType, "message", StringComparison.OrdinalIgnoreCase) ||
                            string.Equals(outItem["role"]?.ToString(), "assistant", StringComparison.OrdinalIgnoreCase) ||
                            string.Equals(outItem["type"]?.ToString(), "message", StringComparison.OrdinalIgnoreCase))
                        {
                            if (outItem["content"] is JsonArray contentArray)
                            {
                                foreach (var c in contentArray)
                                {
                                    var cType = c?["type"]?.ToString();
                                    if (string.Equals(cType, "output_text", StringComparison.OrdinalIgnoreCase))
                                    {
                                        var text = c["text"]?.ToString();
                                        if (!string.IsNullOrEmpty(text))
                                            result.Text += text;
                                    }
                                }
                            }
                        }
                    }

                    // 已处理 Ark 格式，直接返回
                    return;
                }

                // 回退兼容：尝试解析 choices.delta.content（旧 Volcengine 风格）
                var contentValue = parsedData["choices"]?[0]?["delta"]?["content"]?.ToString();

                if (string.IsNullOrEmpty(contentValue))
                    return;

#if false
                /***********************************************************************
                 * 推理模型思考内容
                 * 1. content字段内：Groq（推理后带有换行）(兼容think标签还带有换行情况)
                 * 2. reasoning_content字段内：DeepSeek、硅基流动（推理后带有换行）、第三方服务商
                

                #region 针对content内容中含有推理内容的优化

                if (contentValue.Trim() == "<think>")
                    isThink = true;
                if (contentValue.Trim() == "</think>")
                {
                    isThink = false;
                    // 跳过当前内容
                    return;
                }

                if (isThink)
                    return;

                #endregion

                #region 针对推理过后带有换行的情况进行优化

                // 优化推理模型思考结束后的\n\n符号
                if (string.IsNullOrWhiteSpace(sb.ToString()) && string.IsNullOrWhiteSpace(contentValue))
                    return;

                sb.Append(contentValue);

                #endregion
                 ************************************************************************/
#endif
                result.Text += contentValue;
            }
            catch
            {
                // Ignore
                // * 适配OpenRouter等第三方服务流数据中包含与Volcengine官方API中不同的数据
                // * 如 ": OPENROUTER PROCESSING"
            }
        },option, cancellationToken: cancellationToken);
    }
}