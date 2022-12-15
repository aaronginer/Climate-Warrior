using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using Unity.VisualScripting.FullSerializer;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Assertions;

namespace Dialogue
{
    public class DialogueReader
{
    public string npcNamePrefix = "";
    
    private Dictionary<int, DialogueNode> _nodes = new Dictionary<int, DialogueNode>();
    private DialogueNode _currentNode;

    public DialogueReader(string filename)
    {
        if (!ParseFile(filename))
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
    private bool ParseFile(string filename)
    {
        string dialoguePath = Application.dataPath + Path.AltDirectorySeparatorChar + "Materials/Dialogues/" +
                              filename;
        StreamReader reader = new StreamReader(dialoguePath);


        string line = "";
        while ((line = reader.ReadLine()) != null)
        {
            if (line[0] == '#')
            {
                var split = line.Split(":");
                if (split.Length == 2 && split[0] == "#npc-name" && npcNamePrefix == "")
                {
                    npcNamePrefix = split[1] + ": ";
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
                        id = int.Parse(match.Groups[1].ToString());
                        message = match.Groups[2].ToString();
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
        catch (Exception e)
        {
            Debug.Log("No dialogue node with id 1 in dialogue file.");
            Console.WriteLine(e);
            throw;
        }
    }
}   
}