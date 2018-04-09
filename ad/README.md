# CPSAD主要用来展示线下攻防赛
## 展示内容
- 队伍一血
- 参赛队伍的积分排行榜
- 场景中队伍时时情况
- 比赛的名称
- 比赛倒计时
- 回合队伍积分排行榜
- 比赛回放和 回放进度条 支持拖动和点击实现快进和后退 
- 点击队伍和题目功能以及队伍信息和题目信息的展示功能暂时未开启
- 支持根据队伍当前分数显示多段不同颜色的血条并根据队伍数据时时更新血条（暂时隐藏/打开血条功能操作详见StreamingAssets/changelog.txt）
- 针对配置较低的机型添加了设置性能功能和屏幕分辨率功能并增加了截图功能具体按键功能详见下面的按键说明

## 设置内容  
### websocket链接设置 
- Windows： StreamingAssets/WebSocketSetting.txt
- web: index.html中


		<script type='text/javascript'>
		function getSocketURL() {
			gameInstance.SendMessage("Main", "StartSocket", "ws://202.112.51.152:8050/situation_stream/AD_1_REPLAY");
		}
		</script>

## 按键说明
- M键可以打开设置分辨率和显示性能功能面板
- F键打开FPS显示
- C键加速
- V键复位速度
- P键为屏幕截图
- 图片存放位置：Windows版本  CPSOJ_Windows/oj_Data/

### GameSetting 设置 
- StreamingAssets/GameSettings.json
## 发布命名规范
Windows  ：  可以加版本号 文件名称直接使用下列名称  /  应用名称 为 OJ  /AD



1. CPSAD_Windows_V2.6  


1. CPSOJ_Windows_V2.6

WebGL:      不用加版本号文件名称直接使用下列名称 


1. CPSAD_WebGl


1. CPSOJ_WebGl
## 修改内容详见 StreamingAssets/changelog.txt
	unity  版本 ：5.6.0f3 (64-bit)
