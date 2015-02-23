using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

    public enum Gender { Male, Female, Nonbinary };
    public enum AssignedGender { Male, Female };

    List<string> boysNames;
    List<string> girlsNames;

    Gender gender;
    AssignedGender assignedGender;
    string name;
    string givenName;
    int dysphoria;
    bool ready;
    bool goingOut;
    bool gotClothes;

	// Use this for initialization
	void Start () 
    {
        gender = Gender.Female;
        assignedGender = AssignedGender.Male;
        name = "Amy";
        givenName = "Josh";
       // dysphoria = 0;
        dysphoria = (Random.Range(0, 3));

        boysNames = new List<string>();
        girlsNames = new List<string>();

        string[] boysNamesInput = {"Josh", "Michael", "Christopher", "Matt", "Daniel", "David",
                                      "Andrew", "James", "Justin", "Joseph", "Ryan", "John", "Robert",
                                      "Nick", "Anthony", "Kyle", "Brandon", "Jacob", "William", "Tyler",
                                      "Zach", "Kevin", "Eric", "Steven", "Thomas", "Brian"};
        string[] girlsNamesInput = {"Jessica", "Ashley", "Brittany", "Amanda", "Samantha", "Sarah",
                                       "Stephanie", "Jenn", "Elizabeth", "Lauren", "Megan", "Emily",
                                       "Nicole", "Kayla", "Amber", "Rachel", "Courtney", "Danielle",
                                       "Heather", "Melissa", "Rebecca", "Michelle", "Tiffany", "Chelsea",
                                       "Christina", "Katherine"};

        boysNames.AddRange(boysNamesInput);
        girlsNames.AddRange(girlsNamesInput);
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public string GetVarText(string var)
    {
        if (var.Equals("assignedGender"))
        {
            switch (assignedGender)
            {
                case AssignedGender.Male:
                    return "boy";
                case AssignedGender.Female:
                    return "girl";
                default:
                    return "girl";
            }
        }
        else if (var.Equals("assignedGender2"))
        {
            switch (assignedGender)
            {
                case AssignedGender.Male:
                    return "sir";
                case AssignedGender.Female:
                    return "ma'am";
                default:
                    return "ma'am";
            }
        }
        else if (var.Equals("gender"))
        {
            switch (gender)
            {
                case Gender.Male:
                    return "male";
                case Gender.Female:
                    return "female";
                default:
                    return "nonbinary";
            }
        }
        else if (var.Equals("givenName"))
        {
            return givenName;
        }
        else if (var.Equals("name"))
        {
            return name;
        }
        return "meme";
    }

    public void TakeInput(string inputVar, string input)
    {
        if (inputVar.Equals("name"))
        {
            SetName(input);
        }
    }

    public void ExecuteCommand(string command)
    {
        if (command.Contains("="))
        {
            string lhs = command.Substring(0, command.IndexOf('='));
            string rhs = command.Substring(command.IndexOf('=') + 1);

            if (lhs.Equals("assignedGender"))
            {
                AssignGender(rhs);
            }
            if (lhs.Equals("gender"))
            {
                SetGender(rhs);
            }
            if (lhs.Equals("ready"))
            {
                ready = true;
            }
            if (lhs.Equals("goingOut"))
            {
                goingOut = true;
            }
            if (lhs.Equals("gotClothes"))
            {
                gotClothes = true;
            }
        }
    }

    public bool VerifyConditional(string lhs, string rhs)
    {
        if (rhs[0] == '!')
        {
            return !VerifyConditional(lhs, rhs.Remove(0, 1));
        }
        if (lhs.Equals("assignedGender"))
        {
            return (rhs.Equals("male") && assignedGender == AssignedGender.Male) ||
                   (rhs.Equals("female") && assignedGender == AssignedGender.Female);
        }

        if (lhs.Equals("gender"))
        {
            return (rhs.Equals("male") && gender == Gender.Male) ||
                   (rhs.Equals("female") && gender == Gender.Female) ||
                   (rhs.Equals("nonbinary") && gender == Gender.Nonbinary);
        }

        if (lhs.Equals("dysphoria"))
        {
            return dysphoria == int.Parse(rhs);
        }

        if (lhs.Equals("ready"))
        {
            return ready;
        }

        if (lhs.Equals("goingOut"))
        {
            return goingOut;
        }

        if (lhs.Equals("gotClothes"))
        {
            return gotClothes;
        }
        return false;
    }

    void SetName(string name)
    {
        this.name = name;
        givenName = name;
        while (givenName.Equals(name))
        {
            if (assignedGender == AssignedGender.Male)
            {
                int randomNum = (Random.Range(0, boysNames.Count));
                givenName = boysNames[randomNum];
            }
            else
            {
                int randomNum = (Random.Range(0, girlsNames.Count));
                givenName = girlsNames[randomNum];
            }
        }
    }

    void AssignGender(string gender)
    {
        if (gender.Equals("boy"))
        {
            assignedGender = AssignedGender.Male;
            
        }
        else if (gender.Equals("girl"))
        {
            assignedGender = AssignedGender.Female;
        }
        else
        {
            int randomNum = (Random.Range(0, 2));
            if (randomNum == 0)
            {
                assignedGender = AssignedGender.Male;
            }
            else
            {
                assignedGender = AssignedGender.Female;
            }
        }
    }

    void SetGender(string myGender)
    {
        if (myGender.Equals("boy"))
        {
            gender = Gender.Male;

        }
        else if (myGender.Equals("girl"))
        {
            gender = Gender.Female;
        }
        else
        {
            gender = Gender.Nonbinary;
        }
    }
}
