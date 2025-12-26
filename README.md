# STranslate.Plugin.Translate.Volcengine

<div align="center">

**适用于 STranslate 的火山方舟平台翻译插件**

[![GitHub release](https://img.shields.io/github/release/ybhgl/STranslate.Plugin.Translate.Volcengine.svg)](https://github.com/ybhgl/STranslate.Plugin.Translate.Volcengine/releases)

本项目为开源翻译软件 [STranslate](https://github.com/zggsong/STranslate) 接入了火山方舟平台 [Responses API](https://www.volcengine.com/docs/82379/1569618?lang=zh) 服务，让您可以在 STranslate 中无缝体验火山方舟平台强大的翻译能力。

</div>

---

## ✨ 特性

- 🚀 **无缝集成**：作为 STranslate 的插件，安装后即可在主程序中直接使用。
- 🤖 **模型多样**：支持调用火山方舟平台的 `doubao-seed-translation`、`doubao-seed-1-6-flash` 等多种模型。
- 🔧 **官方 API**：基于火山方舟平台 [Responses API](https://www.volcengine.com/docs/82379/1569618?lang=zh) 开发，稳定可靠。
- 🌐 **超高性能**：得益于火山方舟平台的优质服务，提供快速、准确的翻译体验。

---

## 📦 安装

### 手动下载安装

1.  前往本项目的 [Releases](https://github.com/ybhgl/STranslate.Plugin.Translate.Volcengine/releases) 页面。
2.  下载最新版本的 `STranslate.Plugin.Translate.Volcengine.spkg` 插件文件。
3.  在 STranslate 的 **“设置”** -> **“插件”** 页面，点击 **“添加插件”**，选择下载的文件。

---

## ⚙️ 配置

安装插件后，您需要配置火山方舟的 API 密钥才能使用。

1.  获取火山方舟 API 密钥：
    *   访问 [火山方舟管理控制台](https://console.volcengine.com/ark/)。
    *   开通所需要的模型。
    *   在 **“系统管理”** -> **“API Key 管理”** 页面创建并获取您的 `API Key`。

2.  在 STranslate 中配置：
    *   在 STranslate 的翻译服务的 **“文本翻译”** 中，点击底部的 **“添加”**，选择刚刚安装的 **火山方舟** 插件。
    *   填入您获取的 `API Key`。
    *   根据需要选择您想使用的翻译模型（如 `doubao-seed-translation`）。
    *   保存配置后即可开始使用。

---

## 📖 相关文档

- **[STranslate 主项目](https://github.com/zggsong/STranslate)**
- **[火山方舟 Responses API 文档](https://www.volcengine.com/docs/82379/1569618?lang=zh)**

---

## 🤝 贡献

欢迎提交 Issue 来报告 Bug 或提出新功能建议。如果您想贡献代码，欢迎 Fork 本项目并提交 Pull Request。

