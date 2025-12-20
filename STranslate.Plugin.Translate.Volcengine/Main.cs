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

    public override string? GetSourceLanguage(LangEnum langEnum)
    {
        var isTranslationModel = Settings.Model?.IndexOf("translation", StringComparison.OrdinalIgnoreCase) >= 0;
        
        if (isTranslationModel)
        {
            return langEnum switch
            {
                LangEnum.Auto => "",
                LangEnum.ChineseSimplified => "zh",
                LangEnum.ChineseTraditional => "zh-Hant",
                LangEnum.Cantonese => "zh",
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
                _ => ""
            };
        }
        else
        {
            return langEnum switch
            {
                LangEnum.Auto => "Requires you to identify automatically",
                LangEnum.ChineseSimplified => "Simplified Chinese",
                LangEnum.ChineseTraditional => "Traditional Chinese",
                LangEnum.Cantonese => "Cantonese",
                LangEnum.English => "English",
                LangEnum.Japanese => "Japanese",
                LangEnum.Korean => "Korean",
                LangEnum.French => "French",
                LangEnum.Spanish => "Spanish",
                LangEnum.Russian => "Russian",
                LangEnum.German => "German",
                LangEnum.Italian => "Italian",
                LangEnum.Turkish => "Turkish",
                LangEnum.PortuguesePortugal => "Portuguese",
                LangEnum.PortugueseBrazil => "Portuguese",
                LangEnum.Vietnamese => "Vietnamese",
                LangEnum.Indonesian => "Indonesian",
                LangEnum.Thai => "Thai",
                LangEnum.Malay => "Malay",
                LangEnum.Arabic => "Arabic",
                LangEnum.Hindi => "Hindi",
                LangEnum.MongolianCyrillic => "Mongolian",
                LangEnum.MongolianTraditional => "Mongolian",
                LangEnum.Khmer => "Central Khmer",
                LangEnum.NorwegianBokmal => "Norwegian Bokmål",
                LangEnum.NorwegianNynorsk => "Norwegian Nynorsk",
                LangEnum.Persian => "Persian",
                LangEnum.Swedish => "Swedish",
                LangEnum.Polish => "Polish",
                LangEnum.Dutch => "Dutch",
                LangEnum.Ukrainian => "Ukrainian",
                _ => "Requires you to identify automatically"
            };
        }
    }

    public override string? GetTargetLanguage(LangEnum langEnum)
    {
        var isTranslationModel = Settings.Model?.IndexOf("translation", StringComparison.OrdinalIgnoreCase) >= 0;
        
        if (isTranslationModel)
        {
            return langEnum switch
            {
                LangEnum.Auto => "zh",
                LangEnum.ChineseSimplified => "zh",
                LangEnum.ChineseTraditional => "zh-Hant",
                LangEnum.Cantonese => "zh",
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
                _ => "zh"
            };
        }
        else
        {
            return langEnum switch
            {
                LangEnum.Auto => "Requires you to identify automatically",
                LangEnum.ChineseSimplified => "Simplified Chinese",
                LangEnum.ChineseTraditional => "Traditional Chinese",
                LangEnum.Cantonese => "Cantonese",
                LangEnum.English => "English",
                LangEnum.Japanese => "Japanese",
                LangEnum.Korean => "Korean",
                LangEnum.French => "French",
                LangEnum.Spanish => "Spanish",
                LangEnum.Russian => "Russian",
                LangEnum.German => "German",
                LangEnum.Italian => "Italian",
                LangEnum.Turkish => "Turkish",
                LangEnum.PortuguesePortugal => "Portuguese",
                LangEnum.PortugueseBrazil => "Portuguese",
                LangEnum.Vietnamese => "Vietnamese",
                LangEnum.Indonesian => "Indonesian",
                LangEnum.Thai => "Thai",
                LangEnum.Malay => "Malay",
                LangEnum.Arabic => "Arabic",
                LangEnum.Hindi => "Hindi",
                LangEnum.MongolianCyrillic => "Mongolian",
                LangEnum.MongolianTraditional => "Mongolian",
                LangEnum.Khmer => "Central Khmer",
                LangEnum.NorwegianBokmal => "Norwegian Bokmål",
                LangEnum.NorwegianNynorsk => "Norwegian Nynorsk",
                LangEnum.Persian => "Persian",
                LangEnum.Swedish => "Swedish",
                LangEnum.Polish => "Polish",
                LangEnum.Dutch => "Dutch",
                LangEnum.Ukrainian => "Ukrainian",
                _ => "Requires you to identify automatically"
            };
        }
    }

    public override void Init(IPluginContext context)
    {
        Context = context;
        Settings = context.LoadSettingStorage<Settings>();

        Settings.Prompts.ForEach(Prompts.Add);
    }

    public override void Dispose() => _viewModel?.Dispose();

    /// <summary>
    /// 清理文本末尾的无用空行
    /// </summary>
    /// <param name="text">原始文本</param>
    /// <returns>清理后的文本</returns>
    private string CleanTrailingEmptyLines(string text)
    {
        if (string.IsNullOrEmpty(text)) return text;

        // 使用正则表达式匹配末尾的空行（包括只有空白字符的行）
        return System.Text.RegularExpressions.Regex.Replace(text, @"\s+$", string.Empty);
    }

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

        // 某些翻译模型（名称中含"translation"）不接受 temperature 参数，需单独处理
        var isTranslationModel = model?.IndexOf("translation", StringComparison.OrdinalIgnoreCase) >= 0;

        object content;
        if (isTranslationModel)
        {
            // 严格按照翻译模型要求的格式发送请求：不包含 prompt（使用原始文本）、不包含 temperature/stream/thinking
            content = new
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
                                text = request.Text,
                                translation_options = translationOptions
                            }
                        }
                    }
                }
            };
        }
        else
        {
            content = new
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
                                text = combinedText
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
        }

        var option = new Options
        {
            Headers = new Dictionary<string, string>
            {
                { "Authorization", "Bearer " + (Settings?.ApiKey ?? string.Empty) }
            }
        };

        await Context.HttpService.StreamPostAsync(uriBuilder.Uri.ToString(), content, msg =>
        {
            if (string.IsNullOrEmpty(msg?.Trim())) return;

            var preprocessString = msg.Replace("data:", "").Trim();

            // 终止条件处理
            if (preprocessString.Equals("[DONE]") || 
                preprocessString.Contains("response.completed")) 
                return;

            try
            {
                var parsedData = JsonNode.Parse(preprocessString);
                if (parsedData == null) return;

                // 处理流式响应的增量文本
                if (parsedData["type"]?.ToString() == "response.output_text.delta")
                {
                    var delta = parsedData["delta"]?.ToString();
                    if (!string.IsNullOrEmpty(delta))
                    {
                        result.Text += delta;
                    }
                    return;
                }

                // 处理非流式响应的完整输出
                if (parsedData["output"] is JsonArray outputArray)
                {
                    foreach (var item in outputArray)
                    {
                        // 只处理assistant类型的消息输出
                        if (item?["type"]?.ToString() != "message") continue;
                        if (item?["role"]?.ToString() != "assistant") continue;

                        var contentArray = item["content"] as JsonArray;
                        if (contentArray == null) continue;

                        foreach (var content in contentArray)
                        {
                            // 只处理output_text类型的内容
                            if (content?["type"]?.ToString() != "output_text") continue;
                            
                            var text = content["text"]?.ToString();
                            if (!string.IsNullOrEmpty(text))
                            {
                                result.Text += text;
                            }
                        }
                    }
                }
            }
            catch
            {
                // 忽略解析错误
            }
        }, option, cancellationToken: cancellationToken);

        // 清理末尾的无用空行
        result.Text = CleanTrailingEmptyLines(result.Text);
    }
}
