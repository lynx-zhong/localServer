## 各个proto message后缀意义
* `Req`前端请求
* `Rsp`前端请求回复
* `Res`后端主动同步

## frame.proto
>RequestCode
名字以`Request`开头，后缀不要用上方后缀特殊意义的后缀为后缀（就算带了前端这边解析会自动干掉后缀再链接，没意义）

## MsgIDDefine
prop包名 + RequestCode + 后缀意义

## 通信定义规则
* 先定义`frame.proto`的`RequestCode`
  >通信id定义
* 在定义`xxx.proto`的具体`message`
  >会根据message后缀来推测这个通信是`请求`-`回包`-`同步`