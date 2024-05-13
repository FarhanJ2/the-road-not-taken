@echo off

rem Delete index.html
del index.html

rem Delete TemplateData folder and its contents
rmdir /s /q TemplateData

rem Delete Build folder and its contents
rmdir /s /q Build
