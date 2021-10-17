/*
the general idea on how the computer finds the correct series:
we'll divide the task into 2 subtasks:
1. find all the colors that the series has.
2. once we have all the colors, find the right combination of them.

to complete each sub task, the computer keeps track of all the series possible
and after each guess, he can deduce which series of all the possible combinations are not the one
we're looking for.

*note*
I chose to include only 10 colors in this game, for 2 reasons: 
1.complexity - if the user wants too mamy colors in his game, then the first stage array
will be too large and will slow down the whole algorithem. in addition, if there are more colors,
then the user can choose a larger series size, which means the second stage array length will be
(series length)!, which also will slow down the algorithem.
conclusion: bool pgia isnt a polynomial complexity problem, therefore we only allow up to 10 colors.

2. the user needs to choose distinct colors, if there are too many in the game then it will be hard
to differ between them.
*end note*

complexity: the complexity is the maximum between (series length)! and 10 choose series length
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        int buttonhigh;
        Button[,] Guesses;
        Button[] ColorArray;
        TextBox[,] bolpgia;
        Color y;
        int[][] temp;
        int numOfSets;
        bool alreadyOk = false;
        int shura3 = 0;
        int permutNum;
        int[][] all;
        int[] tmp;
        int[][] temp2;
        int[] saveguess;
        public Form1()
        {
            InitializeComponent();
        }
        private int comb2(int n, int k)
        { // calculates n over k
            int nsum = factorial(n);
            int ksum = factorial(k);
            return nsum/(ksum*(factorial(n-k)));
        }      
        private int[][] generateArr()   
        {
            //  create the sets array
            int numOfSets = comb2(10, Guesses.GetLength(1));
            int[][] sets = new int[numOfSets][];
            for (int g = 0; g < numOfSets; g++)
            {
                sets[g] = new int[Guesses.GetLength(1)];
            }
            return sets;
        }
        private int[][] getAllSubSets(int[] arr)
        {   // initialize the sets array, inserting all possible non repeating, no meaning 
            // to order of colors series to each array in the 2D array
            // length of the 2D array is [10] (num of colors) choose [series length] 
            int[][] sets = generateArr();
            int[] indexes = new int[Guesses.GetLength(1)];
            for (int i = 0; i < Guesses.GetLength(1); i++)
            {
                indexes[i] = i;
            }
            bool finished = false;
            bool found = false;
            int count = 0;
            // once the array is created and initialized, insert all series as described before
            while (!finished)
            {
                for (int t = 0; t < Guesses.GetLength(1); t++)
                {
                    sets[count][t] = arr[indexes[t]];
                }
                int j = Guesses.GetLength(1) - 1;
                int i = 1;
                found = false;
                while (j >= 0 && !found)
                {
                    if (indexes[j] < 10 - i)
                    {
                        found = true;
                        indexes[j]++;
                        for (int k = j + 1; k < Guesses.GetLength(1); k++)
                        {
                            indexes[k] = indexes[j] + k - j;
                        }
                    }
                    else
                    {
                        j--;
                        i++;
                    }
                }
                count++;
                if (!found)
                    finished = true;
            }

            return sets;
        }
        private void PrintArr(int[] arr) 
        { // handles the UI, prints all colors to the users
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] == 1)
                    Guesses[shura3, i].BackColor = Color.Blue;
                else if (arr[i] == 2)
                    Guesses[shura3, i].BackColor = Color.Green;
                else if (arr[i] == 3)
                    Guesses[shura3, i].BackColor = Color.Black;
                else if (arr[i] == 4)
                    Guesses[shura3, i].BackColor = Color.Yellow;
                else if (arr[i] == 5)
                    Guesses[shura3, i].BackColor = Color.Red;
                else if (arr[i] == 6)
                    Guesses[shura3, i].BackColor = Color.Purple;
                else if (arr[i] == 7)
                    Guesses[shura3, i].BackColor = Color.Brown;
                else if (arr[i] == 8)
                    Guesses[shura3, i].BackColor = Color.Pink;
                else if (arr[i] == 9)
                    Guesses[shura3, i].BackColor = Color.Cyan;
                else if (arr[i] == 10)
                    Guesses[shura3, i].BackColor = Color.Coral;
            }
        }
        private int[] clone(int[] x)
        { // clone an array, used to avoid aliasing
            if (x == null)
                return null;
            int[] clone = new int[Guesses.GetLength(1)];
            for (int i = 0; i < Guesses.GetLength(1); i++)
            {
                clone[i] = x[i];
            }
            return clone;
        }  
        private bool inSet(int[] s, int num) 
        {// checks whether a number is in an array, used as a mean to check if a color exists
         // in a given series
            for (int i = 0; i < Guesses.GetLength(1); i++)
            {
                if (s[i] == num)
                    return true;
            }
            return false;
        }
        private void removeBadSets(int[][] sets, int[] guess, int numCorrect, int numOfSets) //מוציא את הסדרות שלא יכולות להיות
        {
            /* the function deletes bad permuation.
            a bad permutation contains more colors of the guess array than the bool+pgia (aka numCorrect)
            explanation: if for example a series A of length 4 has only 2 correct colors in our
            guess, then if another series B contains more than 2 colors of A,
            then B is not the series were looking for, because if they were in the series, we 
            would have gotten more numCorrects
            */
            for (int i = 0; i < numOfSets; i++)
            {
                if (sets[i] != null)
                {
                    int count = 0;
                    for (int j = 0; j < Guesses.GetLength(1); j++)
                    {// counts how many colors the guess and the other series and the sets
                    // have in common
                        if (inSet(sets[i], guess[j]))
                        {
                            count++;
                        }
                    }
                    if (count > numCorrect) // deletes bad permutation
                    {
                        sets[i] = null;
                    }
                }
            }
        }
        private int[] getFirstAvailable(int[][] sets, int numOfSets)
        {
            // returns the next first guess possible
            for (int i = 0; i < numOfSets; i++)
            {
                if (sets[i] != null)
                {
                    return sets[i];
                }
            }
            return null;
        }
        private int[] guessSet(int[][] sets, int numOfSets, ref bool alreadyOk)   
        {
            //deduce bad guesses and preapre the computer next guess, show it to the user
            int p = int.Parse(bolpgia[shura3, 0].Text);
            int b = int.Parse(bolpgia[shura3, 1].Text);
            if (p + b == Guesses.GetLength(1))
            {
                button2.Enabled = false;
                return saveguess;
            }
            removeBadSets(sets, saveguess, b + p, numOfSets);
            int[] guess = clone(getFirstAvailable(sets, numOfSets));
            saveguess = guess;
            shura3++;
            printSet(guess);
            if (b == Guesses.GetLength(1))
                alreadyOk = true;
            return guess;        
        }
        private void printSet(int[] set) // helps printing
        {
            if (shura3 < Guesses.GetLength(0))
            {
                for (int i = 0; i < Guesses.GetLength(1); i++)
                {
                    if (set[i] == 1)
                        Guesses[shura3, i].BackColor = Color.Blue;
                    else if (set[i] == 2)
                        Guesses[shura3, i].BackColor = Color.Green;
                    else if (set[i] == 3)
                        Guesses[shura3, i].BackColor = Color.Black;
                    else if (set[i] == 4)
                        Guesses[shura3, i].BackColor = Color.Yellow;
                    else if (set[i] == 5)
                        Guesses[shura3, i].BackColor = Color.Red;
                    else if (set[i] == 6)
                        Guesses[shura3, i].BackColor = Color.Purple;
                    else if (set[i] == 7)
                        Guesses[shura3, i].BackColor = Color.Brown;
                    else if (set[i] == 8)
                        Guesses[shura3, i].BackColor = Color.Pink;
                    else if (set[i] == 9)
                        Guesses[shura3, i].BackColor = Color.Cyan;
                    else if (set[i] == 10)
                        Guesses[shura3, i].BackColor = Color.Coral;
                }
            }
        }
        private int factorial(int k) // calculates factorial recursively
        {                     
            int factorial = 1;
            for (int i = k; i > 0; i--)
            {
                factorial = factorial * i;
            }
            return factorial;
        }
        private void Permut(int[] permut, int[][] all)
        {
            /*
             once we find all the colors in the series, we need only organise them.
             the function initializes an array with all possible combinations for the correct colors.
             the length of this array will be factorial(series length)
            */
            int[] indexes = new int[Guesses.GetLength(1)];
            for (int r = 0; r < Guesses.GetLength(1); r++)
            {
                indexes[r] = 0;
            }

            int i = 1;
            int j, k;
            k = 0;
            int count = 0;
            while (i < Guesses.GetLength(1))
            {
                if (indexes[i] < i)
                {
                    j = i % 2 * indexes[i];
                    // swap - k as tmp    
                    k = permut[j];
                    permut[j] = permut[i];
                    permut[i] = k;
                    all[count] = clone(permut);

                    count++;
                    indexes[i]++;
                    i = 1;
                }
                else
                {
                    indexes[i] = 0;
                    i++;
                }
           }

        }
        private void removeBadPerms(int[][] perms, int[] guess, int numCorrect, int numPerms)
        {
            /*
             like in the previous guessing phase, we deduce based on how many bools which series
             cannot be the correct one.
             a bad series is one where the amount of colors that are in the same position as
             the colors in the guess array is bigger than the number of bools.
             explanation: if for example a series A of length 4 has only 2 correct placements in our
             guess, then if another series B contains more than 2 colors of A in the same positions,
             then B is not the series were looking for, because if they were in the correct 
             place in the series, we would have gotten more bools.
            */
            for (int i = 0; i < numPerms; i++)
            {
                if (perms[i] != null)
                {
                    int count = 0;
                    for (int j = 0; j < Guesses.GetLength(1); j++)
                    {
                        // counts how many colors are in the same place as in our guess
                        if (perms[i][j] == guess[j]) 
                            count++;
                    }
                    if (count > numCorrect)
                    {
                        perms[i] = null;
                    }
                }

            }
        }
        private int[] guessPerm(int[][] perms, int numPerms)//מנחש את הסדרה
        {
            //removes bad series, and show the user the computer's next guess. also check for win
            int b = int.Parse(bolpgia[shura3, 1].Text);

            if (b == Guesses.GetLength(1))
            {
                bolpgia[shura3 + 1, 1].Enabled = false;
                Win();          
                return saveguess;
            }
            removeBadPerms(perms, saveguess, b, numPerms);
            int[] guess = clone(getFirstAvailable(perms, numPerms));
            saveguess = guess;
            shura3++;
            printSet(guess);
            return guess;
        }
        private void Win()
        {
            MessageBox.Show("i won :D");
            button3.Enabled = false;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            // checks validation, creates the game board accoarding to the input, handles UI 
            try
            {
                int sidra = int.Parse(textBox1.Text);
                if (sidra > 10)
                    MessageBox.Show("to many colors");
                else
                {
                    int i;
                    panel1.Visible = true;

                    Guesses = new Button[int.Parse(textBox2.Text), sidra];
                    ColorArray = new Button[10];
                    bolpgia = new TextBox[Guesses.GetLength(0), 2];
                    int num = 0;
                    buttonhigh = 30;
                    panel1.Height = buttonhigh * Guesses.GetLength(0);
                    panel3.Height = buttonhigh * bolpgia.GetLength(0);
                    for (int j = 0; j < Guesses.GetLength(0); j++)
                    {
                        for (i = 0; i < Guesses.GetLength(1); i++)
                        {
                            Guesses[j, i] = new Button();
                            Guesses[j, i].Width = (panel1.Width / Guesses.GetLength(1));
                            Guesses[j, i].Height = buttonhigh;
                            Guesses[j, i].Location = new Point(Guesses[j, i].Width * i, Guesses[j, i].Height * j);
                            Guesses[j, i].Tag = num.ToString();
                            num++;
                            panel1.Controls.Add(Guesses[j, i]);
                        }
                    }
                    num = 0;
                    for (int j = 0; j < bolpgia.GetLength(0); j++)
                    {
                        for (i = 0; i < bolpgia.GetLength(1); i++)
                        {
                            bolpgia[j, i] = new TextBox();
                            bolpgia[j, i].Width = (panel3.Width / bolpgia.GetLength(1));
                            bolpgia[j, i].Location = new Point(bolpgia[j, i].Width * i, (bolpgia[j, i].Height * j)+(Guesses[j, i].Height * j-bolpgia[j, i].Height * j));
                            bolpgia[j, i].Tag = num.ToString();
                            num++;
                            panel3.Controls.Add(bolpgia[j, i]);
                        }
                    }
                    num = 0;
                    for (int j = 0; j < Guesses.GetLength(0); j++)
                        for (i = 0; i < Guesses.GetLength(1); i++)
                        {
                            Guesses[j, i].Enabled = false;
                        }
                    y = button1.BackColor;
                    int[] options = new int[10];
                    for (i = 1; i < 10 + 1; i++)
                    {
                        options[i - 1] = i;
                    }
                    int[][] sets = getAllSubSets(options);
                    numOfSets = comb2(10, Guesses.GetLength(1));
                    int d = factorial(10);
                    PrintArr(sets[0]);
                    saveguess = clone(sets[0]);
                    temp = sets;
                    button1.Enabled = false;
                    button3.Enabled = false;
                    button2.Enabled = true;
                   
                    textBox1.Enabled = false;
                    textBox2.Enabled = false;
                    for (i = 0; i < bolpgia.GetLength(0); i++)
                        for (int j = 0; j < bolpgia.GetLength(1); j++)
                            bolpgia[i, j].Enabled = false;
                    for (i = 0; i < bolpgia.GetLength(1); i++)
                        bolpgia[shura3, i].Enabled = true;
                    if (panel2.Height > panel1.Height)
                        this.Height = panel2.Height + 80;
                    else
                        this.Height = panel1.Height + 80;
                }
            }
            catch
            {
                MessageBox.Show("not valid numbers");
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            // check user input validation, and if valid then find the computer next guess and show it
            try
            {
                
                int p = int.Parse(bolpgia[shura3,0].Text);
                int b = int.Parse(bolpgia[shura3, 1].Text);
                if (p + b > Guesses.GetLength(1))
                    MessageBox.Show("too many bools or pgia");
                else
                {
                    for (int i = 0; i < bolpgia.GetLength(0); i++)
                        for (int j = 0; j < bolpgia.GetLength(1); j++)
                            bolpgia[i, j].Enabled = false;
                    for (int i = 0; i < bolpgia.GetLength(1); i++)
                    {
                        if (shura3 != Guesses.GetLength(0))                           
                            bolpgia[shura3 + 1, i].Enabled = true;
                    }
                    int[] rightSet = guessSet(temp, numOfSets, ref alreadyOk);
                    if (p + b == Guesses.GetLength(1))
                    {
                        if (alreadyOk)
                            Win();
                        else
                        {
                            for (int i = shura3; i < bolpgia.GetLength(0); i++)
                                bolpgia[i, 0].Enabled = false;
                            permutNum = factorial(Guesses.GetLength(1));
                            all = new int[permutNum][];
                            for (int i = 0; i < permutNum; i++)
                            {
                                all[i] = new int[Guesses.GetLength(1)];
                            }
                            tmp = clone(rightSet);
                            if (tmp != null)
                                Permut(tmp, all);
                            all[permutNum - 1] = rightSet;
                            temp2 = all;
                            button3.Enabled = true;
                            saveguess = rightSet;
                            int[] correct = guessPerm(temp2, permutNum);        
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("invalid");
            }

            }
        private void button3_Click(object sender, EventArgs e)
        {
            // once the computer has guessed all the colors, organsize them
            try
            {
                int b = int.Parse(bolpgia[shura3, 1].Text);
                if (b > Guesses.GetLength(1))
                    MessageBox.Show("too many bools");
                else
                {
                    bolpgia[shura3, 1].Enabled = false;
                    bolpgia[shura3+1, 1].Enabled = true;
                    temp2 = all;
                    int[] correct = guessPerm(temp2, permutNum);
                }
            }
            catch
            {
                MessageBox.Show("invalid number of bools");
            }
            
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            // intilaze
            ColorArray = new Button[10];
            int i;
            int num = 0;
            for (i = 0; i < ColorArray.Length; i++)
            {
                ColorArray[i] = new Button();
                ColorArray[i].Width = panel2.Width;
                ColorArray[i].Height = panel2.Height / ColorArray.Length;
                ColorArray[i].Location = new Point(0, ColorArray[i].Height * i);
                ColorArray[i].Tag = num.ToString();
                num++;
                panel2.Controls.Add(ColorArray[i]);
            }
            ColorArray[0].BackColor = Color.Blue;
            ColorArray[1].BackColor = Color.Green;
            ColorArray[2].BackColor = Color.Black;
            ColorArray[3].BackColor = Color.Yellow;
            ColorArray[4].BackColor = Color.Red;
            ColorArray[5].BackColor = Color.Purple;
            ColorArray[6].BackColor = Color.Brown;
            ColorArray[7].BackColor = Color.Pink;
            ColorArray[8].BackColor = Color.Cyan;
            ColorArray[9].BackColor = Color.Coral;
           
            for (i = 0; i < ColorArray.Length; i++)
            {
                ColorArray[i].Enabled = false;
            }
            button2.Enabled = false;
            button3.Enabled = false;
            
        }
        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
