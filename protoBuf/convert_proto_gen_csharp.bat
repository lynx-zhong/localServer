@echo off

set protoc_path=protoc-3.5.1\protoc
set proto_dir=proto
set csharp_dir=..\localServer\localServer\proto-gen-csharp
set client_dir=..\clientServerTest\Assets\Third\ProtoBuffer

if exist "%client_dir%" (
    del /Q /S %client_dir% > nul
) else (
	mkdir %client_dir%
)

if exist "%csharp_dir%" (
    del /Q /S %csharp_dir% > nul
) else (
	mkdir %csharp_dir%
)

@REM 生成到服务器目录
for /r "%proto_dir%" %%a in (*.proto) do (
	echo %proto_dir%\%%~na.proto
	%protoc_path% --proto_path=%proto_dir% --csharp_out=%csharp_dir% %proto_dir%\%%~na.proto
)

@REM 复制到客户端目录
xcopy /s /e /y "%csharp_dir%\*.*" "%client_dir%\"

pause