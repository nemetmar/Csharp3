// See https://aka.ms/new-console-template for more information
using Hangman;

HangmanGame game = new HangmanGame(10, "tyrannosaurus");

Output.DisplayWelcome();

while(game.CurrentState != GameState.Finished)
{
    game.RunRound();
}
