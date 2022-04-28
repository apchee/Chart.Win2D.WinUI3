using System;

namespace ChartBase.Chart;

public interface IControl: IElement
{
    
    //public IControlContainer ControlContainer { get;}
}


[Flags]
public enum ControlTypes
{
    None = 0b_0000_0000_0000_0000,
    Hovorable = 0b_0000_0000_0000_0001,
    MouseClickable = 0b_0000_0000_0000_0010,
    KeyPressable = 0b_0000_0000_0000_0100
}