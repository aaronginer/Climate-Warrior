using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Dialogue
{
    public class DialogueReader
{
    public string npcName = "";
    
    private Dictionary<int, DialogueNode> _nodes = new Dictionary<int, DialogueNode>();
    private DialogueNode _currentNode;

    public DialogueReader(string dialogueName)
    {
        if (!ParseFile(dialogueName))
        {
            Debug.Log("Error parsing dialogue file.");
            _nodes.Clear();
        }
    }
    
    /*
     * Parses a dialogue file and saves each line as a DialogueNode
     *
     * Each line in the file MUST have the format: [NodeID]"<NPC-message>",[NodeRedirectID1]"<Option1>",[NodeRedirectID1]"<Option1>",...
     * The dialogue file MUST contain a node with ID 1
     *
     * Example dialogue: Materials/Dialogues/testdialogue.txt
     * 
     * A line with no player options marks the end of the conversation.
     */
    private bool ParseFile(string dialogueName)
    {
        string dialoguePath = "Dialogues/" + dialogueName;
        TextAsset fileContent = Resources.Load<TextAsset>(dialoguePath);

        // https://stackoverflow.com/questions/1547476/easiest-way-to-split-a-string-on-newlines-in-net
        string[] lines = fileContent.text.Split(
            new [] { Environment.NewLine },
            StringSplitOptions.None
        );

        foreach (string line in lines)
        {
            if (line[0] == '#')
            {
                var split = line.Split(":");
                if (split.Length == 2 && split[0] == "#npc-name" && npcName == "")
                {
                    npcName = split[1];
                }
                continue;
            }
            
            int id = -1;
            string message = "";
            Dictionary<string, int> options = new Dictionary<string, int>();
            
            string[] splitLine = line.Split("::");
            for (int i = 0; i < splitLine.Length; i++)
            {
                Debug.Log(splitLine[i]);
                try
                {
                    Regex splitter = new Regex("^([0-9]+?)\"(.*?)\"$");
                    Match match = splitter.Match(splitLine[i]);
                    // Parse NPC test
                    if (i == 0)
                    {
                        id = int.Parse(match.Groups[1].ToString());
                        message = match.Groups[2].ToString().Replace("[player-name]", 
                            GameStateManager.Instance.gameState.playerData.name);
                    }
                    // Parse Answer options
                    else 
                    {
                        int optionId = int.Parse(match.Groups[1].ToString());
                        string optionMessage = match.Groups[2].ToString();
                        options.Add(optionMessage, optionId);
                    }
                }
                catch (Exception e)
                {
                    Debug.Log("Error reading dialogue line: " + line);
                    Console.WriteLine(e);
                    throw;
                }
            }

            Debug.Assert(id != -1);
            Debug.Assert(message != "");
            _nodes.Add(id, new DialogueNode(id, message, options));
        }

        try
        {
            _currentNode = _nodes[1];
        }
        catch (Exception e)
        {
            Debug.Log("No dialogue node with id 1 in dialogue file.");
            Console.WriteLine(e);
            throw;
        }

        return true;
    }

    /*
     * Return the current dialogue node
     */
    public DialogueNode GetCurrent()
    {
        return _currentNode;
    }

    /*
     * Update the current dialogue node depending on the players choice
     */
    public void Choice(string option)
    {
        try
        {
            _currentNode = _nodes[_currentNode.GetChoiceId(option)];
        }
        catch (Exception)
        {
            _currentNode = null;
        }
    }
}   
}
