using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public struct Beat{
    public Beat(Transform t, BeatInstruction ty){
        transform = t;
        type = ty;
    }
    public Transform transform;
    public BeatInstruction type;
}

public enum HitType{
    Normal,
    Perfect,
    Sustained,
    Miss
}

public enum BeatInstruction{
    None,
    NormalBeat,
    Sustained
}

[System.Serializable] public struct Chord{
    public BeatInstruction Lane1Instruction;
    public BeatInstruction Lane2Instruction;
    public BeatInstruction Lane3Instruction;
    public BeatInstruction Lane4Instruction;
}