// See https://aka.ms/new-console-template for more information
using Hangman;

/*
Hra sa mne osobne neda vyhrat, lebo neviem zadat pismeno č

Zadej písmeno:
Č
Zadej prosím jedno písmeno, jiné znaky v tajném slově nejsou.
Zadej písmeno:
č
Zadej prosím jedno písmeno, jiné znaky v tajném slově nejsou.

Odporucam pouzivat iba pismena bez diakritiky na hadane slovo
*/
HangmanGame game = new HangmanGame(10, "kočička");

Output.DisplayWelcome();

while (game.CurrentState != GameState.Finished)
{
    game.RunRound();
}
