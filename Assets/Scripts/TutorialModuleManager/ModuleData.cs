using System;

public struct ModuleData
{
    public Type classType;
    public string[] categoryPath;

    public ModuleData(Type classType, string[] categoryPath)
    {
        this.classType = classType;
        this.categoryPath = categoryPath;
    }
}