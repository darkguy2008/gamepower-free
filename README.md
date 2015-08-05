# GamePower Engine

So you want to make games...

 * Quickly?
 * Easily?
 * Effortlessly?

Then look no further! This *may* very well be the game engine you've been looking for in ages.

**Website:** http://gamepower.no-ip.org/

## What?

**GamePower (GP) is a JavaScript HTML5 Canvas-based game engine** developed by DARKGuy/Alemar, designed with speed and ease of use in mind. It contains its very own set of tools (IDE, converters, debuggers) to help you develop that game you've always wanted to do but couldn't because the game engines out there are too complicated to use or require to spend too much time in learning how to use them.

**It uses a super-easy-to-learn pascal-based language** which was used in a *very old* game development engine called DIV Games Studio 2. It's even backwards-compatible with games made with DIV2 (albeit with some few limitations), so if you made some cool games with it and want to port them to HTML5, compile away! ;) (so far, all the tutorials found in the demo and full versions work without problems and exactly as they do in the real MS-DOS version).

## Why?

It was designed out of the frustration it is to make a quick game, without spending too much time in the learning curve of the GUI, the language, the physics, the inner workings, pros and cons of various other game engines out there. Yes, there are lots of other powerful game engines, maybe with better features, portability and so on, but all that stuff does not really help when all you got is just a couple of hours to throw some code and develop a quick game prototype.

The idea is to have the cake and eat it too. The aim is to provide ease of use while also providing a powerful toolkit to make commercial-grade games in very short time, with the least minimum effort.

## How?

GP contains a compiler (invoked by the IDE) which takes the pascal-like code and converts it to a bunch of JavaScript routines, which are then processed by the GP engine at runtime. Fear not, however, it's not as slow as you might think.

GP comes with sample games for you to try and experiment with. They say a code is worth a thousand words (or was a picture? ;)), so here you go, a sample ping-pong game made with GP:

```Delphi
PROGRAM Pong;
GLOBAL
    p1, p2;
BEGIN
    load_fpg("pong.zip");    // Load graphics package
    put_screen(0, 1);        // Set the background
    player(6,   24, _q, _a); // Create player 1 (move with Q and A keys)
    player(314, 24, _p, _l); // Create player 2 (move with P and L keys)
    ball(160, 100, 1, 1);    // Create the ball
END

PROCESS player(x, y, keyUp, keyDown)
BEGIN
    graph = 2;
    LOOP
        IF (key(keyUp) AND y > 24)
            y = y - 4;
        END
        IF (key(keyDown) AND y < 176)
            y = y + 4;
        END
        FRAME;
    END
END

PROCESS ball(x, y, ix, iy);
BEGIN
    graph = 3;
    LOOP
        IF (y == 14 or y == 186)
            iy = -iy;
        END
        IF (collision(TYPE player))
            ix = -ix;
        END
        x = x + ix;
        y = y + iy;
        IF (out_region(id,0))
        	BREAK;
        END
        FRAME;
    END
    ball(160, 100, ix, iy);
END
```

## Requirements

**For developing a game:**
  * Pentium D 3.0 Ghz or better
  * 1 GB RAM or more
  * Hardware acceleration (video drivers installed, doesn't matter if it's an integrated graphics card)
  * Windows 7 or newer
  * .NET Framework 4.5
  * Lots of ideas! :D

**For developing / modifying the engine:**
  * Same as above
  * Visual Studio 2010 or newer
  * C# intermediate-to-advanced Console and WinForms development knowledge.
  * Advanced JavaScript knowledge (closures, callbacks, canvas drawing, JQuery, monkey-patching...)

## Is it free?

**The TL;DR (too long, didn't read) version:**

  * **Free version:** you can do anything you want as long as you share any modifications you make to the engine (and might win a month or two of paid version usage for free), don't get any revenues from the engine or games made with it and avoid making external tools to package your game into some sort of local webhost container (NW.js and such) since the paid version already does that for you (if you wish you can host the game in GP's website, that part is free as long as your game assets aren't huge). It will also include the engine's splash screen (cannot remove it) and will get the newest features a while after they're released in the paid version. **Note: your games can be closed source along with the engine**, but if you modify the engine you **must** open-source those changes.


  * **Paid version:** It requires a monthly fee, and with it you'll get the newest features as soon as they're ready (3D stuff, alpha blending, etc.), your code and assets will be protected as much as it is possible from prying eyes and your games will be able to be played almost anywhere either online or stand-alone as an executable file. You'll also get custom support and **you will be able to sell your games and include ads in them if you want**. What you can't do is sell the engine or the modifications made to it (even though you can enhance it for your own game) or share your service access with someone other than those specified in the license terms you agree to when you acquire this version.

**The long, "I'm a good person and I will read and comply with this" version:**

There are two versions, **Free** and **Paid**:

 * **Free, as in share-alike-free**: You're free to do whatever you want as long as you:
   * **Open-source (share)** any modifications made to the engine, taking in account we might implement those in next versions of the engine. Who knows, you could even win a month or two of paid usage, for free! :).
   * **Don't get any revenues** from any kind of resale, redistribution, all that GPL stuff (this also includes in-game ads, basically any kind of monetization) from the engine or games made with it.
   * **Don't create external tools to pack the game** into a binary form or local web server. The free version is meant for games to be hosted online and played over a browser (node-based ones don't count as a browser either). The paid version offers such alternative, and it's one I've been working really hard on. If you need to host your game, GamePower provides that service for **free** as long as your game assets aren't huge.
  
   However, the free version will:
   
   * **Get the new features the paid version does 1-3 months later.** Critical bugfixes will be applied immediately though.
   * **Only pack your game in HTML format**, with the raw source code (game and engine) exposed. It's up to you to find out how to protect your game code and assets.
   * **Include a GamePower splash screen** prior to initialization. You *could* remove it, but you wouldn't be playing fair then...
   
 * **Paid, when it's serious business**: This version requires a monthly fee, but there are a **lot** of advantages meaning to give value to every little penny you invest in this version, read on!:
   * **Better features**: Alpha transparency, antialiasing, special effects, 3D graphics, compressed game assets, touch gestures... the possibilities are endless!
   * **Monetization**: You can include ads and sell your games without limitations at all.
   * **Online compilation is secure:** While the free version outputs your game in HTML form, the paid compilation service will output a protected form of your game with *custom* obfuscation methods so it's not that easy than with the free version for a misbehaved player to alter your game.
   * **Cross-platform binaries**: The online compilation service will also generate a custom, stand-alone binary for Windows (Linux and Mac soon), so you don't have to worry about hosting your game online somewhere. Also, in a near future, it'll be possible to get an APK for Android Phones too.
   * **Online support:** from the creator itself through a special sub-forum with guaranteed replies in 3-5 business days, depending on the complexity of the issue. 

    Even if everything looks awesome, there are a few caveats:
  
    * **Exclusive**: The access to the service is non-transferrable and applies to an individual, a group or a company, depending on the chosen license.
    * **You can sell your games,** but you can't sell the engine though. You pay for the service usage, not for the rights of the entire commercial engine source code even if you manage to demistify it. You may modify and enhance the paid version engine for your own games, but you cannot sell those modifications.
