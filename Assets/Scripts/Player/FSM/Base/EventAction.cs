using System;

public enum ActionType {
    Default,
    Attack,
}

[Serializable]
public struct EventAction {
    public Action action;
    public ActionType type;
}