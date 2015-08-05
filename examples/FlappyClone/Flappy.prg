program FlappyClone;
local
    delay;
    state;
    choice;
global
    score;
    cursor;
    isGameOver;
    player;
    
    songMenu;
    songGame;
    songGameOver;    
begin

    set_mode(m640x480);
    load_fpg("Flappy.zip");
    load_fnt("FlappyFont.zip");
    
    songMenu = load_song("menu.xm", 1);
    songGameOver = load_song("gameover.mod", 1);
    
    menu();

end

process menu()
begin

    isGameOver = 0;
    let_me_alone();
    state = 0;
    start_scroll(0,0,101,100,0,5);
    title();
    song(songMenu);
    
    loop
    
        scroll.x0 += 1.5;
        scroll.x1 += 1;
    
        if(state == 1)
            option(260, 501, 5, 1);
            option(260+60, 502, 10, 2);
            option(260+120, 503, 15, 3);
            cursor = menuCursor(150, 260, 504, 3);
            state = 2;
        end

        frame;
    end

end

process title()
private
    g  = 2;
    bf = 0.5;
    vy = 1;
begin

    graph = 500;
    x = 640 / 2;
    y = -200;

    loop
        y += vy;
        vy += g;
        if(y > 250-145)
            y = 250-145;
            vy *= -bf;
            bf -= 0.05;
        end
        if(bf <= 0)
            break;
        end
        frame;
    end

    father.state = 1;
    
    loop
        frame;
    end

end

process option(y, graph, delay, i)
begin

    x = 640 / 2;
    loop
        delay--;
        if(delay < 0)
            flags = 4;
            if(delay < -5)
                flags = 0;
                break;
            end
        end
        frame;
    end

    loop
        if(collision(type mouse))
            cursor.choice = i;
            if(mouse.left)
                if(!isGameOver)
                    menuChoice(i);
                else
                    overChoice(i);
                end            
            end
        end
        frame;
    end

end

process menuCursor(x, sy, graph, max)
private 
    kdelay = 2;
begin

    choice = 1;
    delay = 5;
    
    loop
        delay--;
        if(delay < 0)
            if(key(_down))
                delay = kdelay;
                if(choice < max) choice++; end
            end
            if(key(_up))
                delay = kdelay;
                if(choice > 1) choice--; end
            end
            if(key(_enter))
                delay = kdelay;
                break;
            end
        end
            
        y = sy + (60 * (choice-1));
        
        frame;
    end
    
    if(!isGameOver)
        menuChoice(choice);
    else
        overChoice(choice);
    end

end

process menuChoice(choice)
begin

    if(choice == 1)
        NewGame();
        return;
    else if(choice == 2)
        AppToggleFullscreen();
    else if(choice == 3)
        AppExit();
    end
    
    signal(cursor, s_kill);
    cursor = menuCursor(150, 260, 504, 3);

end

process overChoice(choice)
begin

    if(choice == 1)
        NewGame();
    else
        menu();
    end
    
end

process NewGame()
private
    s;
begin

    stop_song();
    
    s = rand(1, 3);
    if(s == 1)
        songGame = load_song("game1.mod");
    else if(s == 2)
        songGame = load_song("game2.xm");
    else if(s == 3)
        songGame = load_song("game3.xm");
    end
    song(songGame);
    
    score = 0;
    isGameOver = false;
    let_me_alone();    
    
    start_scroll(0,0,101,100,0,5);
    scroll.x1 = 320+160;

    counter();
    
end

process counter()
begin

    x = 640/2;
    y = 480/2;
    graph = 505;
    frame(3000);
    graph = 506;
    frame(3000);
    graph = 507;
    frame(3000);
    graph = 508;
    
    player = bird();
    for(i = 0; i < 30; i++)
        wall(960 + (256 * i), rand(10, 14));
    end
    
    frame(3000);
    
end

process bird()
private
    i;
    g  = 0.5;
    vy = 4;
begin

    write    (1, 10,  10, 0, "Score:");
    write_int(1, 145, 10, 0, OFFSET score);
    
    ctype = c_scroll;
    scroll.camera = id;
    graph = 1;
    x = 20;
    y = 50;
    z = -100;
    i = 0;
    loop
        x += 4;
        
        y += vy;
        vy += g;        
        angle -= 10; 
    
        // TODO: Fix angle drawing
        console.log(angle);
        if(mouse.left)
            vy -= 1.5;
            angle += 10;
        end
        
        if(y < 10)
            y = 10;
            vy = 0;
        end
        if(y > 380)
            break;
        end
        
        if(collision(TYPE wall))
            break;
        end
        
        i++;
        if(i > 2)
            graph++;
            i = 0;
        end
        if(graph > 2)
            graph = 1;
        end

    frame;
    end

    vy = -5;
    loop
        y += vy;
        vy += g;
        if(y > 600)
            break;
        end
    frame;
    end
    
    GameOver();

end

process wall(x, graph)
private
    passed;
begin

    passed = false;
    ctype = c_scroll;
    y = 192;
    
    loop
        if(!isGameOver)        
            if(player != null && !passed)
                if(player.x > x)
                    score += 10;
                    passed = true;
                end
            end
            if(scroll.x > x + 640)
                break;
            end
        end
        frame;
    end

end

process GameOver()
private
    g  = 2;
    bf = 0.5;
    vy = 1;
begin

    song(songGameOver);
    graph = 509;
    x = 640 / 2;
    y = -100;

    loop
        y += vy;
        vy += g;
        if(y > 250-144)
            y = 250-144;
            vy *= -bf;
            bf -= 0.05;
        end
        if(bf <= 0)
            break;
        end
        frame;
    end
    
    isGameOver = 1;
    option(260, 510, 5, 1);
    option(260+60, 503, 10, 2);
    cursor = menuCursor(150, 260, 504, 2);

    loop
        frame;
    end

end