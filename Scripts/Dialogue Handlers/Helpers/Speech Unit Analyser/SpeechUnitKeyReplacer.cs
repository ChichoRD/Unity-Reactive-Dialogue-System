using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using Object = UnityEngine.Object;

public class SpeechUnitKeyReplacer :  MonoBehaviour, ISpeechUnitAnalyser
{
    private static readonly Regex s_KeyPattern = new Regex(@"\{(?<key>[^\}]+)\}");
    public Dictionary<string, object> KeyReplacements { get; private set; } = new Dictionary<string, object>();

    [RequireInterface(typeof(ISpeechUnitAnalyser))]
    [SerializeField] private Object _nextSpeechUnitAnalyserObject;
    public ISpeechUnitAnalyser Next => _nextSpeechUnitAnalyserObject as ISpeechUnitAnalyser;

    public SpeechDialogueUnit Analyse(SpeechDialogueUnit speechDialogueUnit)
    {
        const string GROUP_NAME = "key";
        string message = speechDialogueUnit.Message;
        MatchCollection matches = s_KeyPattern.Matches(message);

        foreach (Match match in matches.Cast<Match>())
        {
            string key = match.Groups[GROUP_NAME].Value;
            if (!KeyReplacements.TryGetValue(key, out object replacement)) continue;

            message = message.Replace(match.Value, replacement.ToString());
        }

        SpeechDialogueUnit s = new SpeechDialogueUnit(message,
                                                      speechDialogueUnit.WaitTime,
                                                      speechDialogueUnit.OverrideTypingSpeed,
                                                      speechDialogueUnit.TypingSpeed);

        return Next?.Analyse(s) ?? s;
    }
}