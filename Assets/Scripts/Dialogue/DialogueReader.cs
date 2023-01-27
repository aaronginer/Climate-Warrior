using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Dialogue
{
    public class DialogueReader
{
    public string npcName = "";
    
    private Dictionary<int, DialogueNode> _nodes = new Dictionary<int, DialogueNode>();
    private DialogueNode _currentNode;
    private string _endAction = "";

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
     * Each line in the file MUST have the either one of these formats:
     * 1)   [NodeID]"<NPC-message>",[NodeRedirectID1]"<Option1>",[NodeRedirectID1]"<Option1>",...
     *      which defines a dialogue node
     * 2)   #npc-name:<name>
     *      which defines the NPCs name
     * 3)   #actions:<action1>,<action2>,<action3>,<action4>
     *      which defines the mission actions on a dialogue choice (you can provide * actions)
     * 4)   #end-action:<action>
     *      defines the action taken when dialogue is finished
     * The dialogue file MUST contain a node with ID 1
     *
     * Additional features:
     * 1)   [player-name] is substituted with the name of the player
     *
     * Example dialogue: Materials/Dialogues/testdialogue.txt
     * 
     * A line with no player options marks the end of the conversation.
     */
    private bool ParseFile(string dialogueName)
    {
        string dialoguePath = "Dialogues/" + dialogueName;
        TextAsset fileContent = Resources.Load<TextAsset>(dialoguePath);

        if (fileContent == null)
        {
            return false;
        }

        // https://stackoverflow.com/questions/1547476/easiest-way-to-split-a-string-on-newlines-in-net
        string[] lines = fileContent.text.Split(
            new string[] { "\r\n", "\r", "\n" },
            StringSplitOptions.None
        );
        
        DialogueNode previous = null;
        foreach (string line in lines)
        {
            if (line.Length == 0) continue;
            
            if (line[0] == '#')
            {
                var split = line.Split(":");
                if (split.Length == 2 && split[0] == "#npc-name" && npcName == "")
                {
                    npcName = split[1];
                }
                else if (split.Length == 2 && split[0] == "#actions")
                {
                    string[] actions = split[1].Split(",");
                    previous?.UpdateActions(actions);
                }
                else if (split.Length == 2 && split[0] == "#end-action")
                {
                    _endAction = (split[1]);
                }
                continue;
            }
            
            int id = -1;
            string message = "";
            Dictionary<string, int> options = new Dictionary<string, int>();
            
            string[] splitLine = line.Split("::");
            for (int i = 0; i < splitLine.Length; i++)
            {
                try
                {
                    Regex splitter = new Regex("^([0-9]+?)\"(.*?)\"$");
                    Match match = splitter.Match(splitLine[i]);
                    // Parse NPC test
                    if (i == 0)
                    {
                        PlayerData pData = GameStateManager.Instance.gameState.playerData;
                        string name = pData.name;
                        string addressAs = pData.gender == PlayerData.Gender.Male ? "man"
                            : pData.gender == PlayerData.Gender.Female ? "woman" : "person";
                        id = int.Parse(match.Groups[1].ToString());
                        message = match.Groups[2].ToString().Replace("[player-name]", 
                            pData.name).Replace("[player-gender]", addressAs);
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
            previous = new DialogueNode(id, message, options);
            _nodes.Add(id, previous);
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

    public string GetEndAction()
    {
        return _endAction;
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
