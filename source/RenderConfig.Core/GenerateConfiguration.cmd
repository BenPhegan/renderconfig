move Configuration.cs old.cs
xsd Configuration.xsd /c /namespace:RenderConfig.Core 
copy Configuration.cs Configuration.designer.cs /y
move old.cs Configuration.cs
