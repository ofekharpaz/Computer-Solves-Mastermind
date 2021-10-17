# Computer-Solves-Mastermind


The code is in the file Form1.cs, and was written in C#.

How classic Mastermind works: one player chooses a combination of 4 non-repeating colors out of a set of colors. The second player job is to guess that exact combination of colors, in the same order aswell. Each turn the guessing player make his guess, Then the codemaker rates his guess with 2 diffrent kind of pegs: white means 1 color of the guess is in the series, and black means 1 color is in the series and in the correct location. Now, the guessing player needs to make an educated guess based on which pegs the codemaker placed.




In this project I built Mastermind between you and the computer, where you are the codemaker and the computer needs to guess the series of colors you chose. First, enter how many colors are in this series (up to 10, more on that later) and how many guesses you allow the computer to guess.








Then, starts the guessing algorithm.
The general idea on how the computer finds the correct series:
we'll divide the task into 2 subtasks:
1. find all the colors that the series has.
2. once we have all the colors, find the right combination of them.



To complete each sub task, the computer keeps track of all the series possible
and after each guess, he can deduce which series of all the possible combinations are not the one
we're looking for. A bad permutation contains more colors of the guess array than the black+white pegs
Explanation: If for example a series A of length 4 has only 2 correct colors in ourguess, then if another series B contains more than 2 colors of A,then B is not the series were looking for, because if they were in the series, we would have gotten more black+white pegs.







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

Complexity: the complexity is the maximum between (series length)! and 10 choose series length.


