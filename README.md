# PartyMaster_Script, scripting assets for the JustDance GameJam (2016)

## Waddat?

For the end of the Just Dance 2017 project, the dev team has 3 days to work as
small pre-defined teams (4-6 people) on a game prototype, which had to be related
to music and run on Android.

Here I store the Unity scripting components of the resulting game. I cannot share the whole
project since the soundbank contained assets from Just Dance. And, as we did not
have any artist on our team, you can guess why I do not want to include the 2D
assets either.

## Disclaimer

I know parts of the code are *horrendous*, please do not consider this as my usual
coding quality. Because honestly, if I did code that way on a daily basis,
colleagues would already have thrown me through the window during code reviews.

So yeah, do not dwell on that.

## Wadda game aboot?

PartyMaster is a two-player game about a quite common neighboring issue : one
wants to throw the party of the year, while the other one just wants a quiet night.

Each player is in his flat and, once the music kicks in, has limited time (four music bars) to pick two actions (move to another room, knock on the ceiling, yell, stomp...) that will be resolved simultaneously.

The resolution phase dequeues the picked actions, for which each player has to reproduce
a short rhythmic pattern. The more in sync hits the player gets, the stronger the effect.
Playing actions right above/beneath the other player can boost or hinder your own actions, so pay attention to where the sound comes from to guess in which room the other must be!

Successful actions from the PartyMaster attract more people to the party, while actions from
the Caretaker shoo them away. Game's over once 100 people are partying, or everyone went away, or the music's over.

## Why sharing this then?

Although we had too much ambition, and our team was unexpectedly short of two members
due to bad luck, I'm quite proud of what we achieved for our first game jam ever.

So I'll just leave that here for safekeeping, as a reminder of those three days of fun.

I worked with [Slimane Dellaoui](http://www.slimanedellaoui.com/) for all things
sound-related, and he's an awesome dude, so extra kudos to him!
