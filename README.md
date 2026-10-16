# AIAA4435_project

基于 VirtuWander 的电脑端虚拟博物馆项目。

## 电脑端运行

- 原项目版本：Unity 2022.3.0f1；本机安装版本：2022.3.62f3。
- 用 Unity Hub 打开此文件夹，等待首次资源导入。
- 打开 `Assets/Scenes/DesktopMuseum.unity`，点击 Play。
- WASD 移动，鼠标转向，左 Shift 加速，Esc 释放鼠标，单击恢复控制。
- 电脑端关闭 VR 初始化、AI 导览界面和语音输入，参观不需要 Python 服务或 API 密钥。
- 移动受场景已有 NavMesh 限制；原始 `Scene_2D.unity` 和 `ChatGPT Sample.unity` 保留供后续开发。
- 菜单 `Museum > Build Windows Desktop` 生成 `Builds/Windows/AIAA4435_project.exe`。

## 可选 Python AI 服务

在项目根目录使用 PowerShell：

```powershell
python -m venv Service/chatgpt_env
./Service/chatgpt_env/Scripts/python.exe -m pip install -r Service/requirements.txt
$env:OPENAI_API_KEY = "填入自己的密钥"
./Service/chatgpt_env/Scripts/python.exe Service/chatgpt_service.py
```

健康检查地址：`http://127.0.0.1:5000/chatgpt/status`。
服务沿用旧版 OpenAI SDK 接口，固定 `openai==0.28.1`；原始依赖列表留在
`Service/requirements-legacy.txt`，当前环境完整依赖记录在 `Service/requirements-lock.txt`。
AI 问答还需要可用密钥和有权限使用的模型；健康检查通过不代表在线模型调用已验证。
原 `object_detection.py` 是独立的 Mask R-CNN 实验，需要额外模型权重和依赖，不属于参观运行链路。
原语音合成改从 `AWS_ACCESS_KEY_ID`、`AWS_SECRET_ACCESS_KEY` 环境变量读取凭据。
不要把密钥写入代码或上传仓库。

## 原项目内容

保留原仓库的博物馆 FBX、画作纹理、导航网格、头像及演示场景。
电脑端入口复制自 `Scene_2D`，增加 `DesktopMuseumController`。
Unity 缓存、构建产物、Python 虚拟环境不提交 Git；Unity `.meta` 文件必须保留。

[原 README 参考视频](https://www.youtube.com/watch?v=CthF8c8qk4c&t=977s)

已移除上游失效且项目脚本未使用的 Ready Player Me WebView Git 依赖。
