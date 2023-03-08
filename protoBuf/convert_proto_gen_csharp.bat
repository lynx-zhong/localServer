@echo off

set protoc_path=protoc-3.5.1\protoc
set proto_dir=proto
set csharp_dir=..\localServer\localServer\proto-gen-csharp

for /r "%proto_dir%" %%a in (*.proto) do (
	echo %proto_dir%\%%~na.proto
	%protoc_path% --proto_path=%proto_dir% --csharp_out=%csharp_dir% %proto_dir%\%%~na.proto
)

@REM protoc --proto_path=IMPORT_PATH --cpp_out=DST_DIR 
@REM --java_out=DST_DIR 
@REM --python_out=DST_DIR 
@REM --go_out=DST_DIR 
@REM --ruby_out=DST_DIR 
@REM --objc_out=DST_DIR 
@REM --csharp_out=DST_DIR path/to/file.proto

pause