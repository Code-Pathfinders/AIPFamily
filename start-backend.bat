@echo off
echo ========================================
echo 启动家族族谱管理系统 - 后端
echo ========================================
echo.

cd Backend\WebAPI

echo 正在启动后端服务...
echo API地址: http://localhost:5000
echo Swagger文档: http://localhost:5000
echo.

dotnet run

pause
