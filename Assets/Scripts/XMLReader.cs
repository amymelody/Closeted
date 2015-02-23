using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml;

public class XMLReader : MonoBehaviour {
    public string xmlFile;
    public GUIStyle textStyle;
    public GUIStyle inputTextStyle;
    public string dialogueID;
    public int fontSize;
    public float textPadding;
    public float textYPercentage;
    XmlDocument root;
    int itemIndex;
    bool takingInput;
    bool advanceOnUpdate;
    string text;
    string inputText;
    float textY;

	// Use this for initialization
	void Start() 
    {
        // Get the file into the document

        root = new XmlDocument();
        root.Load(xmlFile);

        if (dialogueID.Equals(""))
        {
            dialogueID = "0";
        }

        itemIndex = 0;
        textStyle.fontSize = fontSize;
        inputTextStyle.fontSize = fontSize;

        textY = Screen.height * textYPercentage;

        AdvanceDialogue();
	}
	
	// Update is called once per frame
	void Update() 
    {
        if (advanceOnUpdate)
        {
            advanceOnUpdate = false;
            AdvanceDialogue();
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            AdvanceDialogue();
        }
        if (takingInput)
        {
            foreach (char c in Input.inputString)
            {
                if (c == "\b"[0] && text.Length > 0)
                {
                    text = text.Remove(text.Length - 1);
                }
                else
                {
                    text += c;
                }
            }
        }
	}

    void AdvanceDialogue()
    {
        GameObject selector = GameObject.Find("Selector(Clone)");
        if (selector) //Made a selection
        {
            inputText = "";
            int choice = selector.GetComponent<Selector>().choice;
            XmlNode choiceNode = root.SelectSingleNode("game/dialogue[@id='" + dialogueID + "']").ChildNodes[itemIndex - 1].ChildNodes[choice];
            Destroy(selector);
            foreach (XmlNode action in choiceNode.ChildNodes)
            {
                DoAction(action);
            }
        }

        if (takingInput)
        {
            inputText = "";
            XmlNode inputNode = root.SelectSingleNode("game/dialogue[@id='" + dialogueID + "']").ChildNodes[itemIndex - 1];
            string inputVar = inputNode.InnerText;
            GetComponent<Player>().TakeInput(inputVar, text);
            takingInput = false;
        }


        XmlNodeList dialogueNodes = root.SelectSingleNode("game/dialogue[@id='" + dialogueID + "']").ChildNodes;
        if (itemIndex >= dialogueNodes.Count)
        {
            Debug.Log("Goodbye");
            Application.Quit();
        }

        XmlNode dialogueNode = dialogueNodes[itemIndex];
        DoDialogue(dialogueNode);

        itemIndex++;

        if (DoAction(dialogueNode))
        {
            advanceOnUpdate = true;
        }

    }

    void DoDialogue(XmlNode dialogueNode)
    {
        if (dialogueNode.Name.Equals("text"))
        {
            text = ParseText(dialogueNode.InnerText);
        }
        else if (dialogueNode.Name.Equals("makeChoice"))
        {
            text = "    ";
            inputText = ParseText(dialogueNode.Attributes[0].Value);
            foreach (XmlNode choice in dialogueNode.ChildNodes)
            {
                text += ParseText(choice.Attributes[0].Value);
                text += "\n    ";
            }
            GameObject selector = (GameObject)Instantiate(Resources.Load<GameObject>("Prefabs/Selector"));
            selector.transform.position = new Vector3(-5.8f, -1.85f, 0f);
            selector.GetComponent<Selector>().numChoices = dialogueNode.ChildNodes.Count;
        }
        else if (dialogueNode.Name.Equals("input"))
        {
            inputText = ParseText(dialogueNode.Attributes[0].Value);
            text = "";
            takingInput = true;
        }
    }

    string ParseText(string text)
    {
        string returnText = text;
        while (returnText.Contains("[") && returnText.Contains("]"))
        {
            int firstIndex = returnText.IndexOf("[");
            int secondIndex = returnText.IndexOf("]");
            returnText = returnText.Replace(returnText.Substring(firstIndex, secondIndex - firstIndex + 1),
                GetComponent<Player>().GetVarText(returnText.Substring(firstIndex + 1, secondIndex - 1 - firstIndex)));
        }
        return returnText;
    }

    bool DoAction(XmlNode action)
    {
        if (action.Name.Equals("command"))
        {
            string command = action.InnerText;
            GetComponent<Player>().ExecuteCommand(command);
            return true;
        }
        else if (action.Name.Equals("jump"))
        {
            dialogueID = action.InnerText;
            itemIndex = 0;
            return true;
        }
        else if (action.Name.Equals("color"))
        {
            ChangeTextColor(action.InnerText);
            return true;
        }
        else if (action.Name.Equals("if"))
        {
            if (GetComponent<Player>().VerifyConditional(action.Attributes[0].Name, action.Attributes[0].Value))
            {
                DoDialogue(action.ChildNodes[0]);
                return DoAction(action.ChildNodes[0]);
            }
            return true;
        }
        return false;
    }

    void ChangeTextColor(string color)
    {
        if (color.Equals("green"))
        {
            Color green = new Color(0.5f, 1.0f, 0.5f);
            textStyle.normal.textColor = green;
            inputTextStyle.normal.textColor = green;
        }
        if (color.Equals("cyan"))
        {
            textStyle.normal.textColor = Color.cyan;
            inputTextStyle.normal.textColor = Color.cyan;
        }
        if (color.Equals("white"))
        {
            textStyle.normal.textColor = Color.white;
            inputTextStyle.normal.textColor = Color.white;
        }
        if (color.Equals("gray"))
        {
            textStyle.normal.textColor = Color.gray;
            inputTextStyle.normal.textColor = Color.gray;
        }
    }

    void OnGUI()
    {
        GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
        GUI.Label(new Rect(textPadding, 0,
                           Screen.width - 2f * textPadding, textY - textPadding), inputText, inputTextStyle);
        GUI.Label(new Rect(textPadding, textY,
                           Screen.width - 2f * textPadding, Screen.height - textY - textPadding), text, textStyle);
        GUI.EndGroup();
    }
}
