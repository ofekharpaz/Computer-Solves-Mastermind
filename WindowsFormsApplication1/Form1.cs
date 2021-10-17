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
        Button[,] BolPgia;
        Button[] ColorArray;
        TextBox[,] bolpgia;
        int[] options;
        int num = 1;
        int numOfBol = 0;
        Color y;
        int[][] sets;
        int[][] temp;
        int numOfSets;
        bool alreadyOk = false;
        int shura3 = 0;
        int sidra;
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
        {
            int nsum = factorial(n);
            int ksum = factorial(k);
            return nsum/(ksum*(factorial(n-k)));
        }      
        private int[][] generateArr()  //sets יוצר את המערך 
        {
            int numOfSets = comb2(10, Guesses.GetLength(1));
            int[][] sets = new int[numOfSets][];
            for (int g = 0; g < numOfSets; g++)
            {
                sets[g] = new int[Guesses.GetLength(1)];
            }
            return sets;
        }
        private int[][] getAllSubSets(int[] arr)  //את כל הסדרות האפשריות setsמכניס ל 
        {
            int[][] sets = generateArr();
            int[] indexes = new int[Guesses.GetLength(1)];
            for (int i = 0; i < Guesses.GetLength(1); i++)
            {
                indexes[i] = i;
            }
            bool finished = false;
            bool found = false;
            int count = 0;
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
        private void PrintArr(int[] arr) // מדפיס את המערך
        {
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
        private int[] clone(int[] x)  //משכפל את המערך
        {
            if (x == null)
                return null;
            int[] clone = new int[Guesses.GetLength(1)];
            for (int i = 0; i < Guesses.GetLength(1); i++)
            {
                clone[i] = x[i];
            }
            return clone;
        }  
        private bool inSet(int[] s, int num) //משווה בין שני ערכים
        {
            for (int i = 0; i < Guesses.GetLength(1); i++)
            {
                if (s[i] == num)
                    return true;
            }
            return false;
        }
        private void removeBadSets(int[][] sets, int[] guess, int numCorrect, int numOfSets) //מוציא את הסדרות שלא יכולות להיות
        {
            for (int i = 0; i < numOfSets; i++)
            {
                if (sets[i] != null)
                {
                    int count = 0;
                    for (int j = 0; j < Guesses.GetLength(1); j++)
                    {
                        if (inSet(sets[i], guess[j]))
                        {
                            count++;
                        }
                    }
                    if (count > numCorrect)
                    {
                        sets[i] = null;
                    }
                }
            }
        }
        private int[] getFirstAvailable(int[][] sets, int numOfSets)//null מחזיר את הערך הראשון שאינו 
        {
            for (int i = 0; i < numOfSets; i++)
            {
                if (sets[i] != null)
                {
                    return sets[i];
                }
            }
            return null;
        }
        private int[] guessSet(int[][] sets, int numOfSets, ref bool alreadyOk)   //מנחש את הסדרה
        {
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
        private void printSet(int[] set) // מדפיס את המערך
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
        private int factorial(int k) //מחשב עצרת
        {                     
            int factorial = 1;
            for (int i = k; i > 0; i--)
            {
                factorial = factorial * i;
            }
            return factorial;
        }
        private void Permut(int[] permut, int[][] all)//את כל הסדרות האפשריות all מכניס למערך 
        {
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
        private void removeBadPerms(int[][] perms, int[] guess, int numCorrect, int numPerms)//מוציא את הסדרות שלא יכולות להיות
        {
            for (int i = 0; i < numPerms; i++)
            {
                if (perms[i] != null)
                {
                    int count = 0;
                    for (int j = 0; j < Guesses.GetLength(1); j++)
                    {
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
        private void button1_Click(object sender, EventArgs e)// יוצר את הלוח
        {
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
        private void button2_Click(object sender, EventArgs e)//מנחש את הסדרה
        {
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
        private void button3_Click(object sender, EventArgs e)// מסדר את הסדרה
        {
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
