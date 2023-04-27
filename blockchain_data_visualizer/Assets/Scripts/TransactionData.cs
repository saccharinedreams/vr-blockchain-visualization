using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class TransactionData
{
    public string op;
    public Transaction x;
}

[Serializable]
public class Transaction
{
    public string hash;
    public Input[] inputs;
    public Output[] @out;
}

[Serializable]
public class PrevOut
{
    public string addr;
}

[Serializable]
public class Input
{
    public PrevOut prev_out;
}

[Serializable]
public class Output
{
    public string addr;
    public long value;
}