namespace Domain;

public enum UnitType
{
    Default, Mass, Capacity
}

public enum Status
{
    Default, 
    Active,
    Inactive,
    Maintenance,
    Disposed
}

public enum ActionType
{
    Created,   
    Filled,    
    Emptied,   
    Edited     
}