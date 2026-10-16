# 本地验证记录（2026-10-16）

- 本机 Unity：`D:\Unity 2022.3.62f3\Editor\Unity.exe`。原工程为 2022.3.0f1，已用本机 2022.3 LTS 导入并成功构建。
- 默认电脑场景：`Assets/Scenes/DesktopMuseum.unity`，来自原始 `Scene_2D.unity`。
- 静态资源检查：场景 GUID 引用全部可解析；包含博物馆 FBX、画作材质与纹理。
- Unity 场景检查：13 个根对象，没有缺失脚本，导航网格 201 个顶点。
- Windows 构建成功；程序启动后自动导航移动 2 个场景单位通过。测试覆盖场景加载和导航移动，不等同于人工按键逐项验收或完整展馆逐房检查。
- Python：`Service/chatgpt_env`，Python 3.13.5；`pip check` 通过；真实 HTTP 健康检查返回 200 和 `{"status":"ok"}`。
- AI 在线问答、Whisper、AWS Polly 未实测，需要用户自己的凭据；电脑端参观不依赖这些服务。
- 原对象识别实验缺少 `mask_rcnn_balloon.h5` 权重，不属于电脑端参观功能。

## 已修复的原项目问题

- 删除残缺 `.git`，重新初始化 `main` 并添加指定 origin，README 首次提交已推送。
- 移除已无法访问的 Ready Player Me WebView 依赖；Unity 已将内置包升级为本机编辑器兼容版本。
- 原 Python 依赖列表缺少 fuzzywuzzy，且 OpenAI 无上限版本会与旧 API 冲突；改为服务实际依赖，另保留原清单。
- 清理代码内硬编码 OpenAI 和 AWS 凭据；改读环境变量。
- 新增默认 Windows 场景、WASD/鼠标控制、鼠标捕获/释放、导航边界限制，停用桌面 XR 加载器和自动 AI 导览。
- 按加载顺序启用玩家 NavMeshAgent，避免在导航数据载入前创建 Agent。

## 复现

- 双击 `Builds/Windows/AIAA4435_project.exe` 参观，或在 Unity 打开电脑端场景按 Play。
- WASD 移动；鼠标转向；左 Shift 加速；Esc 释放鼠标；单击恢复控制。
- 菜单 `Museum > Build Windows Desktop` 重新构建。
- `Service/chatgpt_env/Scripts/python.exe Service/smoke_test.py` 重跑服务健康检查。
- 自动运行场景移动检查：从项目根目录启动 exe，附加 `-museum-smoke-test` 参数；程序输出日志标记并自动退出，渲染截图保存到 `Logs/desktop-museum.png`。
- 构建产物必须连同同目录的数据文件夹和 DLL 一起保留，不能只复制 exe。

用户已明确确认提交并推送完整工程；Unity 缓存、Python 虚拟环境和本地构建产物不纳入版本控制。

