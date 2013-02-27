using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Dough
{
    public class Field
    {
        public int height;
        public int width;
        public int[,] blocks;
        public LinkTarget[,] links;
        public bool[,] locked;
        public int[,] deleteTimers;
        public int blockWidth;
        public int blockHeight;
        public int halfWidth;
        public int halfHeight;
        public int startX1;
        public int startX2;
        public TimeSpan bumpTime;
        public TimeSpan dropTime;
        public List<ShinyLocation> shinies;
        public List<ShinyLocation> drawBlocks;
        public List<ShinyLocation> nullBlocks;
        public int currentLevel = 1;
        public Random random;
        public bool vortexMode = false;

        public Field(Random random)
        {
            height = 20;
            width = 20;
            blocks = new int[width, height];
            links = new LinkTarget[width, height];
            locked = new bool[width, height];
            deleteTimers = new int[width, height];
            blockWidth = 25;
            blockHeight = 25;
            halfWidth = 12;
            halfHeight = 12;
            startX1 = 8;
            startX2 = 12;
            bumpTime = new TimeSpan(0, 0, 10); //Set to null to stop increase from occurring
            dropTime = new TimeSpan(0, 0, 0, 0, 1000);
            shinies = new List<ShinyLocation>();
            drawBlocks = new List<ShinyLocation>();
            nullBlocks = new List<ShinyLocation>();
            this.random = random;

            currentLevel = 1;
        }

        private void generateRow(int y, int startX = 0, int endX = Int32.MaxValue, bool suppressDuplicates = false)
        {
            if (endX == Int32.MaxValue)
                endX = width;

            //Generate a row of random blocks
            for (int i = startX; i < endX; i++)
            {
                if (suppressDuplicates)
                {
                    while(blocks[i, y] == 0 || (i != 0 && blocks[i-1, y] == blocks[i, y]) || (y != height - 1 && blocks[i, y] == blocks[i, y + 1]))
                        blocks[i,y] = random.Next(4) + 1;
                }
                else
                    blocks[i, y] = random.Next(4) + 1;

            }
        }

        private void generateLockedRow(int y)
        {
            //Generate a row of random blocks
            for (int i = 0; i < width; i++)
            {
                locked[i, y] = true;
                blocks[i, y] = random.Next(4) + 1;
            }
        }

        private void generateQuickKillColorTower(int x, int y)
        {
            //Generate a tower at the specified x that ends at y.  This tower will have three colors in a row all the way to the top
            //Allowing the player to quickly destroy it
            int currentCount = 0;
            int currentColor = random.Next(4) + 1;
            int previousColor = currentColor;
            for (int i = y; i < height; i++)
            {
                blocks[x, i] = currentColor;
                currentCount++;
                if (currentCount == 3)
                {
                    currentCount = 0;
                    previousColor = currentColor;
                    while(previousColor == currentColor) //Generate a different color this time
                        currentColor = random.Next(4) + 1;
                }
            }
        }

        private void generateLinkedStairs(int startX, int startY, int stairCount, bool upward = true, bool matchColors = false)
        {
            for (int i = 0; i < stairCount; i++)
            {
                int currentX = startX + i;
                int currentY = startY - i;
                if (!upward)
                    currentY = startY + i;

                int firstColor = random.Next(4) + 1;
                int secondColor = random.Next(4) + 1;
                if (matchColors)
                    secondColor = firstColor;

                blocks[currentX, currentY] = firstColor;
                blocks[currentX + 1, currentY] = secondColor;
                links[currentX, currentY] = new LinkTarget(currentX + 1, currentY);
                links[currentX + 1, currentY] = new LinkTarget(currentX, currentY);
            }
        }

        private List<int> generateRandomNumbers(int n, int min, int max, bool suppressRepeats)
        {
            List<int> numbers = new List<int>();

            if ((max - min) + 1 < n && suppressRepeats)
                throw new Exception("There are not enough valid numbers in this range to generate non-repeated random numbers");

            for (int i = 0; i < n; i++)
            {
                int temp = random.Next(max - min + 1) + min;

                if (suppressRepeats)
                {
                    while(numbers.Contains(temp))
                        temp = random.Next(max - min + 1) + min;
                }

                numbers.Add(temp);
            }

            return numbers;
        }

        public void generateRandomDistributedRow(int y)
        {
            //Generate a row that includes spaces
            if (y == height - 1)
                for (int i = 0; i < width; i++)
                    blocks[i, y] = random.Next(5);
            else
                for (int i = 0; i < width; i++)
                    if (blocks[i, y + 1] != 0) //Don't put blocks on top of blank spaces
                        blocks[i, y] = random.Next(5);
        }

        public void loadLevel(int level)
        {
            vortexMode = false;
            if (currentLevel == 1)
                generateLevel1();
            else if (currentLevel == 2)
                generateLevel2();
            else if (currentLevel == 3)
                generateLevel3();
            else if (currentLevel == 4)
                generateLevel4();
            else if (currentLevel == 5)
                generateLevel5();
            else if (currentLevel == 6)
                generateLevel6();
            else if (currentLevel == 7)
                generateLevel7();
            else if (currentLevel == 8)
                generateLevel8();
            else if (currentLevel == 9)
                generateLevel9();
            else if (currentLevel == 10)
                generateLevel10();
            else if (currentLevel == 11)
                generateLevel11();
            else if (currentLevel == 12)
                generateLevel12();
            else if (currentLevel == 13)
                generateLevel13();
            else if (currentLevel == 14)
                generateLevel14();
            else if (currentLevel == 15)
                generateLevel15();
            else if (currentLevel == 16)
                generateLevel16();
            else if (currentLevel == 17)
                generateLevel17();
            else if (currentLevel == 18)
                generateLevel18();
            else if (currentLevel == 19)
                generateLevel19();
            else if (currentLevel == 20)
                generateLevel20();
            else if (currentLevel == 21)
                generateLevel21();
            else if (currentLevel == 22)
                generateLevel22();
            else if (currentLevel == 23)
                generateLevel23();
            else if (currentLevel == 24)
                generateLevel24();
            else if (currentLevel == 25)
                generateLevel25();
        }

        public void setLevel(int level)
        {
            this.currentLevel = level;
            loadLevel(currentLevel);
        }

        public void generateLevel1()
        {
            generateRow(height - 1);
            generateRow(height - 2);
            generateRow(height - 3);
            generateRow(height - 4);
            generateRandomDistributedRow(height - 5);

            shinies.Clear();
            shinies.Add(new ShinyLocation(random.Next(width), height - 4));
            shinies.Add(new ShinyLocation(random.Next(width), height - 3));
        }

        public void generateLevel2()
        {
            generateRow(height - 1);
            generateRow(height - 2);
            generateRow(height - 3);
            generateRow(height - 4);

            List<int> towerXs = generateRandomNumbers(6, 0, width - 1, true);
            for (int i = 0; i <= 3; i++)
                generateQuickKillColorTower(towerXs[i], 8);

            shinies.Clear();
            for (int i = 0; i <= 3; i++)
                shinies.Add(new ShinyLocation(towerXs[i], 8));
            for (int i = 4; i < 6; i++)
                shinies.Add(new ShinyLocation(towerXs[i], height - 4));
        }

        public void generateLevel3()
        {
            for (int i = 5; i < height - 1; i++)
                generateRow(i);

            List<int> x = generateRandomNumbers(1, 0, width - 1, true);
            generateQuickKillColorTower(x[0], 4);

            shinies.Clear();
            shinies.Add(new ShinyLocation(x[0], 4));
        }

        public void generateLevel4()
        {
            generateRow(height - 1);
            generateRow(height - 2);
            generateQuickKillColorTower(0, height - 3);
            generateQuickKillColorTower(width - 1, height - 5);

            generateLinkedStairs(0, height - 4, 10);
            generateLinkedStairs(10, height - 14, 9, false);

            shinies.Clear();
            shinies.Add(new ShinyLocation(10, height - 2));
        }

        public void generateLevel5()
        {
            generateRow(height - 1);
            generateRow(height - 2);
            generateRow(height - 3);
            generateRow(height - 4);
            int towerX = random.Next(width - 5) + 2;
            generateQuickKillColorTower(towerX, 7);
            generateQuickKillColorTower(towerX + 3, 7);
            blocks[towerX, 7] = random.Next(4) + 1;
            blocks[towerX, 8] = random.Next(4) + 1;
            blocks[towerX, 9] = random.Next(4) + 1;
            blocks[towerX - 1, 10] = blocks[towerX, 10];
            blocks[towerX - 1, 9] = random.Next(4) + 1;
            blocks[towerX - 2, 10] = blocks[towerX, 10];
            links[towerX, 10] = new LinkTarget(towerX - 1, 10);
            links[towerX - 1, 10] = new LinkTarget(towerX, 10);
            links[towerX - 2, 10] = new LinkTarget(towerX - 1, 9);
            links[towerX - 1, 9] = new LinkTarget(towerX - 2, 10);
            blocks[towerX + 2, 11] = random.Next(4) + 1;
            links[towerX + 2, 11] = new LinkTarget(towerX + 3, 11);
            links[towerX + 3, 11] = new LinkTarget(towerX + 2, 11);

            //Make sure the lower blocks in the tower are not a match
            for (int i = 11; i < height; i++)
            {
                while (blocks[towerX, i] == blocks[towerX, i - 1])
                    blocks[towerX, i] = random.Next(4) + 1;

                while (blocks[towerX + 3, i] == blocks[towerX + 3, i - 1])
                    blocks[towerX + 3, i] = random.Next(4) + 1;
            }

            //Make sure the horizontal blocks below the tower are not a match
            for (int i = height - 4; i < height; i++)
            {
                if (towerX - 1 != -1)
                    while (blocks[towerX - 1, i] == blocks[towerX, i])
                        blocks[towerX - 1, i] = random.Next(4) + 1;

                if (towerX + 4 < this.width)
                    while (blocks[towerX + 4, i] == blocks[towerX + 3, i])
                         blocks[towerX + 4, i] = random.Next(4) + 1;
            }
            

            while (blocks[towerX, 11] == blocks[towerX, 10])
                blocks[towerX, 11] = random.Next(4) + 1;
            while (blocks[towerX, 9] == blocks[towerX, 10])
                blocks[towerX, 9] = random.Next(4) + 1;

            shinies.Add(new ShinyLocation(towerX, 10));

            int shinyLocationOne = towerX;
            int shinyLocationtwo = towerX;
            while (shinyLocationOne == towerX || shinyLocationOne == towerX + 3)
                shinyLocationOne = random.Next(width);
            while (shinyLocationtwo == towerX || shinyLocationtwo == towerX + 3)
                shinyLocationtwo = random.Next(width);

            shinies.Add(new ShinyLocation(shinyLocationOne, height - 4));
            shinies.Add(new ShinyLocation(shinyLocationtwo, height - 3));
        }

        public void generateLevel6()
        {
            //Create two normal rows on the bottom
            shinies.Clear();
            generateRow(height - 1);
            generateRow(height - 2);

            //A tower without matches on the far left and on the far right
            for (int i = height - 2; i >= 5; i--)
            {
                while (blocks[0,i] == 0 || blocks[0, i] == blocks[0, i + 1])
                    blocks[0, i] = random.Next(4) + 1;
                while (blocks[width - 1, i] == 0 || blocks[width - 1, i] == blocks[width - 1, i + 1])
                    blocks[width - 1, i] = random.Next(4) + 1;
            }

            //Make sure no blocks adjacent to the tower match it:
            for (int i = height - 1; i >= height - 2; i--)
            {
                while (blocks[1, i] == blocks[0, i])
                    blocks[1, i] = random.Next(4) + 1;
                while (blocks[width - 2, i] == blocks[width - 1, i])
                    blocks[width - 2, i] = random.Next(4) + 1;
            }

            //A three layer ceiling that only has a one-width entrance
            for (int i = 1; i < width - 2; i++) //Create the blocks
            {
                while (blocks[i, 5] == 0 || blocks[i, 5] == blocks[i - 1, 5]) //Make sure it doesn't match the preceding block
                    blocks[i, 5] = random.Next(4) + 1;
                while (blocks[i, 6] == 0 || blocks[i, 6] == blocks[i, 5] || blocks[i - 1, 6] == blocks[i, 6]) //Make sure it doesn't match the preceding block or the one above it
                    blocks[i, 6] = random.Next(4) + 1;
                while (blocks[i, 7] == 0 || blocks[i, 7] == blocks[i, 6] || blocks[i, 7] == blocks[i - 1, 7]) //etc.
                    blocks[i, 7] = random.Next(4) + 1;
            }
            //Create the initial link
            links[0, 7] = new LinkTarget(1, 7);
            links[1, 7] = new LinkTarget(0, 7);
            for (int i = 1; i < width - 3; i++) //Create the links
            {
                links[i, 6] = new LinkTarget(i + 1, 7);
                links[i + 1, 7] = new LinkTarget(i,6 );
            }

            //Decrease the amount of time before a rise occurs to 20 seconds
            bumpTime = new TimeSpan(0, 0, 30);

            //Add one shiny to the top row of the bottom
            shinies.Add(new ShinyLocation(random.Next(10) + 5, height - 2));

        }

        public void generateLevel7()
        {
            //Make a few bottom rows
            generateRow(height - 1);
            generateRow(height - 2);
            generateRow(height - 3);
            generateRow(height - 4);

            //Put some shinies on the rows
            shinies.Clear();
            shinies.Add(new ShinyLocation(random.Next(width), height - 4));
            shinies.Add(new ShinyLocation(random.Next(width), height - 3));

            //Put a row near the top to be cleared
            int topColor = random.Next(4) + 1;
            int startX = random.Next(width - 4);
            blocks[startX, 5] = topColor;
            blocks[startX + 1, 5] = topColor;
            blocks[startX + 2, 5] = topColor;
            locked[startX, 5] = true;
            locked[startX + 1, 5] = true;
            locked[startX + 2, 5] = true;
            shinies.Add(new ShinyLocation(startX + 1, 5));

            //Put some 2x2 squares randomly that will have to be dodged
            List<int> yLocations = new List<int>();
            yLocations.Add(6);
            yLocations.Add(8);
            yLocations.Add(8);
            yLocations.Add(11);
            yLocations.Add(11);
            foreach (int y in yLocations)
            {
                int x = -1;
                while (x == -1 || blocks[x, y] != 0 || blocks[x + 1, y] != 0)
                    x = random.Next(width - 1);

                blocks[x, y] = random.Next(4) + 1;
                blocks[x + 1, y] = random.Next(4) + 1;
                blocks[x, y + 1] = random.Next(4) + 1;
                blocks[x + 1, y + 1] = random.Next(4) + 1;
                locked[x, y] = true;
                locked[x + 1, y] = true;
                locked[x, y + 1] = true;
                locked[x + 1, y + 1] = true;
            }

            //Slow down the movement slightly
            bumpTime = new TimeSpan(0, 0, 20);
        }

        public void generateLevel8()
        {
            //Need to have some gimmick where suspending your own block is required
            //My idea at this point is to have the shiny block on the bottom side of something so you
            //have to clear it from below
            //To the side is also a possibility, but that is too simple imo

            //Generate some random rows at the bottom
            generateRandomDistributedRow(height - 1);
            generateRandomDistributedRow(height - 2);
            generateRandomDistributedRow(height - 3);
            generateRandomDistributedRow(height - 4);

            //Generate three three-tall pillars that will have shinies at the bottom
            List<int> shinyColors = generateRandomNumbers(3, 1, 4, true);
            shinies.Clear();
            for (int i = 0; i < 3; i++)
            {
                int y = -1;
                int x = -1;

                while (x == -1 || y == -1 ||
                    blocks[x, y] != 0 || blocks[x, y + 1] != 0 || blocks[x, y + 2] != 0)
                {
                    x = random.Next(width);
                    y = random.Next(8) + 6;
                }

                blocks[x, y] = random.Next(4) + 1;
                blocks[x, y + 1] = random.Next(4) + 1;
                blocks[x, y + 2] = shinyColors[i];
                shinies.Add(new ShinyLocation(x, y + 2));
                locked[x, y] = true;
                locked[x, y + 1] = true;
                locked[x, y + 2] = true;

            }

            //Generate some three-wide platforms to be annoying
            for (int i = 0; i < 2; i++)
            {
                int y = -1;
                int x = -1;

                while (x == -1 || y == -1 ||
                    blocks[x, y] != 0 || blocks[x + 1, y] != 0 || blocks[x + 2, y] != 0)
                {
                    x = random.Next(width - 2);
                    y = random.Next(height - 10) + 6;
                }

                blocks[x, y] = random.Next(4) + 1;
                blocks[x + 1, y] = random.Next(4) + 1;
                blocks[x + 2, y] = random.Next(4) + 1;
                locked[x, y] = true;
                locked[x + 1, y] = true;
                locked[x + 2, y] = true;
            }
        }

        public void generateLevel9()
        {
            //Create a mini tower at the bottom right to be the shiny
            generateRow(height - 1);
            generateQuickKillColorTower(16, height - 3);
            generateQuickKillColorTower(14, height - 3);
            generateQuickKillColorTower(12, height - 3);
            shinies.Clear();
            shinies.Add(new ShinyLocation(16, height - 2));
            shinies.Add(new ShinyLocation(14, height - 2));
            shinies.Add(new ShinyLocation(12, height - 2));

            //Create towers that go near the top on far left and far right
            for (int i = 4; i < height; i++)
            {
                while (blocks[0, i] == 0 || blocks[0, i] == blocks[0, i - 1])
                    blocks[0, i] = random.Next(4) + 1;
                while (blocks[width - 1, i] == 0 || blocks[width - 1, i] == blocks[width - 1, i - 1])
                    blocks[width - 1, i] = random.Next(4) + 1;
            }

            //Create ledges that go back and forth on the way down
            for (int i = 1; i < width - 1; i++)
            {
                while (blocks[i, 5] == 0 || blocks[i, 5] == blocks[i - 1, 5])
                    blocks[i, 5] = random.Next(4) + 1;
                while (blocks[i, 6] == 0 || blocks[i, 6] == blocks[i - 1, 6])
                    blocks[i, 6] = random.Next(4) + 1;
                while (blocks[i, 10] == 0 || blocks[i, 10] == blocks[i - 1, 10])
                    blocks[i, 10] = random.Next(4) + 1;
                while (blocks[i, 11] == 0 || blocks[i, 11] == blocks[i - 1, 11])
                    blocks[i, 11] = random.Next(4) + 1;

                locked[i, 5] = true;
                locked[i, 6] = true;
                locked[i, 10] = true;
                locked[i, 11] = true;
            }

            //put holes in the ledges
            blocks[width - 2, 5] = 0;
            blocks[width - 3, 5] = 0;
            blocks[width - 4, 5] = 0;
            blocks[width - 2, 6] = 0;
            blocks[width - 3, 6] = 0;
            blocks[width - 4, 6] = 0;
            blocks[1, 10] = 0;
            blocks[2, 10] = 0;
            blocks[3, 10] = 0;
            blocks[1, 11] = 0;
            blocks[2, 11] = 0;
            blocks[3, 11] = 0;
            locked[width - 2, 5] = false;
            locked[width - 3, 5] = false;
            locked[width - 4, 5] = false;
            locked[width - 2, 6] = false;
            locked[width - 3, 6] = false;
            locked[width - 4, 6] = false;
            locked[1, 10] = false;
            locked[2, 10] = false;
            locked[3, 10] = false;
            locked[1, 11] = false;
            locked[2, 11] = false;
            locked[3, 11] = false;

        }

        public void generateLevel10()
        {
            //At this point, the user knows all the controls.
            //Put together two difficult but standard puzzles before moving on to new mechanics

            //Make the fuse and bomb on the right side:
            int fuseColor = random.Next(4) + 1;
            List<int> otherColors = generateRandomNumbers(3, 1, 4, true);
            int bottomColor = otherColors[0];
            otherColors.Add(otherColors[1]);
            otherColors.Add(otherColors[2]);
            blocks[width - 3, height - 1] = fuseColor;
            blocks[width - 4, height - 1] = fuseColor;
            blocks[width - 5, height - 1] = fuseColor;
            blocks[width - 2, height - 2] = bottomColor;
            links[width - 2, height - 2] = new LinkTarget(width - 3, height - 1);
            links[width - 3, height - 1] = new LinkTarget(width - 2, height - 2);
            blocks[width - 2, height - 3] = bottomColor;
            shinies.Clear();
            for (int i = 0; i < 4; i++)
            {
                int thisHeight = height - 3 - i;
                blocks[width - 4, thisHeight] = otherColors[i + 1];
                blocks[width - 3, thisHeight] = otherColors[i + 1];
                blocks[width - 2, thisHeight - 1] = otherColors[i + 1];
                blocks[width - 1, thisHeight] = otherColors[i + 1];
                locked[width - 4, thisHeight] = true;
                locked[width - 3, thisHeight] = true;
                locked[width - 1, thisHeight] = true;
                shinies.Add(new ShinyLocation(width - 1, thisHeight));
            }

            //Create the trap on the left side
            blocks[0, height - 1] = random.Next(4) + 1;
            while (blocks[2, height - 1] == 0 || blocks[2, height - 1] == blocks[0, height - 1])
                blocks[2, height - 1] = random.Next(4) + 1;
            for (int i = 2; i < 6; i++)
                while (blocks[0, height - i] == 0 || blocks[0, height - i] == blocks[0, height - i + 1])
                    blocks[0, height - i] = random.Next(4) + 1;
            blocks[0, height - 6] = blocks[0, height - 5];
            blocks[0, height - 7] = blocks[0, height - 5];
            shinies.Add(new ShinyLocation(0, height - 5));
            while (blocks[1, height - 5] == 0 || blocks[1, height - 5] == blocks[2, height - 1])
                blocks[1, height - 5] = random.Next(4) + 1;
            blocks[1, height - 6] = random.Next(4) + 1;
            while (blocks[2, height - 6] == 0 || blocks[2, height - 6] == blocks[2, height - 1])
                blocks[2, height - 6] = random.Next(4) + 1;
            for(int i =7; i<10; i++)
                while(blocks[2, height - i] == 0 || blocks[2, height - i] == blocks[2, height - i + 1])
                    blocks[2, height - i] = random.Next(4) + 1;
            links[0, height - 5] = new LinkTarget(1, height - 5);
            links[1, height - 5] = new LinkTarget(0, height - 5);
            links[1, height - 6] = new LinkTarget(2, height - 6);
            links[2, height - 6] = new LinkTarget(1, height - 6);
            shinies.Add(new ShinyLocation(2, height - 1));

            //Create quick kill pillars in the sky (Player should probably take care of these first)
            List<int> pillarX = generateRandomNumbers(3, 0, width - 1, true);
            for(int i=0; i<3; i++)
            {
                int x = pillarX[i];
                blocks[x, 3] = otherColors[i]; //Use this previous color arrangement so that they don't match colors
                blocks[x, 4] = otherColors[i];
                blocks[x, 5] = otherColors[i];
                locked[x, 3] = true;
                locked[x, 4] = true;
                locked[x, 5] = true;
            }

            //Create the two pits in the middle
            for (int i = 1; i < 9; i++)
            {
                blocks[8, height - i] = random.Next(4) + 1;
                blocks[10, height - i] = random.Next(4) + 1;
                blocks[12, height - i] = random.Next(4) + 1;
            }
            while (blocks[9, height - 1] == 0 || blocks[9, height - 1] == blocks[8, height - 1] || blocks[9, height - 1] == blocks[10, height - 1])
                blocks[9, height - 1] = random.Next(4) + 1;
            while(blocks[11, height - 1] == 0 || blocks[11, height - 1] == blocks[10, height - 1] || blocks[11, height - 1] == blocks[12, height -1])
                blocks[11, height - 1] = random.Next(4) + 1;
            shinies.Add(new ShinyLocation(9, height - 1));
            shinies.Add(new ShinyLocation(11, height - 1));
        }


        public void generateLevel11()
        {
            //Create a bottom row that is all shinies
            generateRow(height - 1);
            shinies.Clear();
            for (int i = 0; i < width; i++)
                shinies.Add(new ShinyLocation(i, height - 1));
        }

        public void generateLevel12()
        {
            //Introduction to Draw blocks level

            //Pyramid on the bottom
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < (8 - 2 * i); j++)
                {
                    drawBlocks.Add(new ShinyLocation(j + 6 + i, height - i - 1));
                }
            }
            for (int i = 0; i < 3; i++)
            {
                drawBlocks.Add(new ShinyLocation(9, height - 5 - i));
                drawBlocks.Add(new ShinyLocation(10, height - 5 - i));
            }

            blocks[7, height - 1] = 4;
            blocks[10, height - 1] = 4;
            blocks[10, height - 2] = 4;

            //Diagonals in the air
            drawBlocks.Add(new ShinyLocation(3, height - 5));
            drawBlocks.Add(new ShinyLocation(4, height - 6));

            drawBlocks.Add(new ShinyLocation(16, height - 5));
            drawBlocks.Add(new ShinyLocation(15, height - 6));
        }

        public void generateLevel13()
        {
            //Create the arch
            for (int i = 0; i < 3; i++)
            {
                drawBlocks.Add(new ShinyLocation(6, height - i - 1));
                drawBlocks.Add(new ShinyLocation(14, height - i - 1));
                for (int j = 7; j < 14; j++)
                    nullBlocks.Add(new ShinyLocation(j, height - i - 1));
            }

            drawBlocks.Add(new ShinyLocation(7, height - 4));
            drawBlocks.Add(new ShinyLocation(13, height - 4));
            for (int i = 8; i < 13; i++)
            {
                nullBlocks.Add(new ShinyLocation(i, height - 4));
                drawBlocks.Add(new ShinyLocation(i, height - 5));
            }

            //Create the room atop the arch
            for (int i = 0; i < 3; i++)
            {
                drawBlocks.Add(new ShinyLocation(8, height - 6 - i));
                drawBlocks.Add(new ShinyLocation(12, height - 6 - i));
                nullBlocks.Add(new ShinyLocation(9, height - 6 - i));
                nullBlocks.Add(new ShinyLocation(10, height - 6 - i));
                nullBlocks.Add(new ShinyLocation(11, height - 6 - i));
            }

            for (int i = 8; i < 13; i++)
                drawBlocks.Add(new ShinyLocation(i, height - 9));

        }

        public void generateLevel14()
        {
            for (int y = 1; y < 11; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if ((x == 3 && y == 1) ||
                        (x == 4 && y == 7) ||
                        (x == 3 && y == 9) ||
                        (x == 6 && y == 1) ||
                        (x == 11 && y == 1) ||
                        (x == 12 && y == 2) ||
                        (x == 12 && y == 3) ||
                        (x == 13 && y == 1))
                    {
                        drawBlocks.Add(new ShinyLocation(x, height - y));
                        continue;
                    }

                    nullBlocks.Add(new ShinyLocation(x, height - y));
                }
            }
        }

        public void generateLevel15()
        {
            //Create the first puzzle on the left
            int triggerColor = random.Next(4) + 1;
            for (int i = 1; i < 4; i++)
            {
                blocks[i, height - 1] = random.Next(4) + 1;
                blocks[i, height - 2] = triggerColor;
                nullBlocks.Add(new ShinyLocation(i, height - 2));
            }
            nullBlocks.Add(new ShinyLocation(4, height - 1));
            drawBlocks.Add(new ShinyLocation(4, height - 2));
            drawBlocks.Add(new ShinyLocation(5, height - 2));
            drawBlocks.Add(new ShinyLocation(4, height - 3));
            blocks[4, height - 3] = random.Next(4) + 1;
            links[4, height - 3] = new LinkTarget(3, height - 2);
            links[3, height - 2] = new LinkTarget(4, height - 3);
            blocks[5, height - 1] = random.Next(4) + 1;
            
            //Create the middle puzzle
            drawBlocks.Add(new ShinyLocation(7, height - 1));
            drawBlocks.Add(new ShinyLocation(8, height - 2));
            drawBlocks.Add(new ShinyLocation(8, height - 3));
            drawBlocks.Add(new ShinyLocation(8, height - 4));
            drawBlocks.Add(new ShinyLocation(9, height - 1));
            drawBlocks.Add(new ShinyLocation(9, height - 4));
            drawBlocks.Add(new ShinyLocation(9, height - 5));
            drawBlocks.Add(new ShinyLocation(10, height - 2));
            drawBlocks.Add(new ShinyLocation(10, height - 3));
            nullBlocks.Add(new ShinyLocation(8, height - 1));
            nullBlocks.Add(new ShinyLocation(9, height - 2));
            nullBlocks.Add(new ShinyLocation(9, height - 3));

            //Create the right puzzle
            int rightColor = random.Next(4) + 1;
            while (rightColor == triggerColor)
                rightColor = random.Next(4) + 1;
            blocks[13, height - 1] = rightColor;
            blocks[14, height - 1] = rightColor;
            blocks[15, height - 1] = rightColor;
            blocks[15, height - 2] = rightColor;
            blocks[15, height - 3] = rightColor;
            shinies.Clear();
            shinies.Add(new ShinyLocation(15, height - 1));
            drawBlocks.Add(new ShinyLocation(15, height - 2));

            //Create the annoyance platform on the right
            blocks[18, height - 4] = rightColor;
            blocks[19, height - 4] = rightColor;
            locked[18, height - 4] = true;
            locked[19, height - 4] = true;
            nullBlocks.Add(new ShinyLocation(18, height - 5));
            nullBlocks.Add(new ShinyLocation(19, height - 5));
        }

        public void generateLevel16()
        {
            //Freebie puzzle
            int currentCount = 0;
            List<int> colors = generateRandomNumbers(4, 1, 4, true);
            List<ShinyLocation> smilyLocations = new List<ShinyLocation>(); //Create a list of Xs and Ys to later loop through and add/lock blocks
            smilyLocations.Add(new ShinyLocation(8, height - 1));
            smilyLocations.Add(new ShinyLocation(9, height - 1));
            smilyLocations.Add(new ShinyLocation(10, height - 1));
            smilyLocations.Add(new ShinyLocation(11, height - 1));
            smilyLocations.Add(new ShinyLocation(12, height - 2));
            smilyLocations.Add(new ShinyLocation(13, height - 2));
            smilyLocations.Add(new ShinyLocation(14, height - 3));
            smilyLocations.Add(new ShinyLocation(15, height - 4));
            smilyLocations.Add(new ShinyLocation(15, height - 5));
            smilyLocations.Add(new ShinyLocation(16, height - 6));
            smilyLocations.Add(new ShinyLocation(16, height - 7));
            smilyLocations.Add(new ShinyLocation(16, height - 8));
            smilyLocations.Add(new ShinyLocation(16, height - 9));
            smilyLocations.Add(new ShinyLocation(15, height - 10));
            smilyLocations.Add(new ShinyLocation(15, height - 11));
            smilyLocations.Add(new ShinyLocation(14, height - 12));
            smilyLocations.Add(new ShinyLocation(13, height - 13));
            smilyLocations.Add(new ShinyLocation(12, height - 13));
            smilyLocations.Add(new ShinyLocation(11, height - 14));
            smilyLocations.Add(new ShinyLocation(10, height - 14));
            smilyLocations.Add(new ShinyLocation(9, height - 14));
            smilyLocations.Add(new ShinyLocation(8, height - 14));
            smilyLocations.Add(new ShinyLocation(7, height - 13));
            smilyLocations.Add(new ShinyLocation(6, height - 13));
            smilyLocations.Add(new ShinyLocation(5, height - 12));
            smilyLocations.Add(new ShinyLocation(4, height - 11));
            smilyLocations.Add(new ShinyLocation(4, height - 10));
            smilyLocations.Add(new ShinyLocation(3, height - 9));
            smilyLocations.Add(new ShinyLocation(3, height - 8));
            smilyLocations.Add(new ShinyLocation(3, height - 7));
            smilyLocations.Add(new ShinyLocation(3, height - 6));
            smilyLocations.Add(new ShinyLocation(4, height - 5));
            smilyLocations.Add(new ShinyLocation(4, height - 4));
            smilyLocations.Add(new ShinyLocation(5, height - 3));
            smilyLocations.Add(new ShinyLocation(6, height - 2));
            smilyLocations.Add(new ShinyLocation(7, height - 2));
            smilyLocations.Add(new ShinyLocation(8, height - 10));
            smilyLocations.Add(new ShinyLocation(11, height - 10));
            smilyLocations.Add(new ShinyLocation(7, height - 6));
            smilyLocations.Add(new ShinyLocation(12, height - 6));
            smilyLocations.Add(new ShinyLocation(8, height - 5));
            smilyLocations.Add(new ShinyLocation(9, height - 5));
            smilyLocations.Add(new ShinyLocation(10, height - 5));
            smilyLocations.Add(new ShinyLocation(11, height - 5));

            foreach (ShinyLocation sl in smilyLocations)
            {
                blocks[sl.x, sl.y] = colors[currentCount];
                locked[sl.x, sl.y] = true;
                currentCount++;
                if (currentCount == 4)
                    currentCount = 0;
                drawBlocks.Add(new ShinyLocation(sl.x, sl.y));
            }

            List<ShinyLocation> nullRanges = new List<ShinyLocation>();
            nullRanges.Add(new ShinyLocation(8, 11)); //These are used as placeholder objects for a range of x1 to x2 (y is derived from index of object)
            nullRanges.Add(new ShinyLocation(6, 13));
            nullRanges.Add(new ShinyLocation(5, 14));
            nullRanges.Add(new ShinyLocation(5, 14));
            nullRanges.Add(new ShinyLocation(4, 15));
            nullRanges.Add(new ShinyLocation(4, 15));
            nullRanges.Add(new ShinyLocation(4, 15));
            nullRanges.Add(new ShinyLocation(4, 15));
            nullRanges.Add(new ShinyLocation(5, 14));
            nullRanges.Add(new ShinyLocation(5, 14));
            nullRanges.Add(new ShinyLocation(6, 13));
            nullRanges.Add(new ShinyLocation(8, 11));
            

            for(int i=0; i<nullRanges.Count; i++)
            {
                for (int j = nullRanges[i].x; j <= nullRanges[i].y; j++)
                {
                    int tempX = j;
                    int tempY = height - 2 - i;
                    if ((tempX == 7 && tempY == height - 6) ||
                        (tempX == 12 && tempY == height - 6) ||
                        (tempX == 8 && tempY == height - 5) ||
                        (tempX == 9 && tempY == height - 5) ||
                        (tempX == 10 && tempY == height - 5) ||
                        (tempX == 11 && tempY == height - 5) ||
                        (tempX == 8 && tempY == height - 10) ||
                        (tempX == 11 && tempY == height - 10))
                        continue;
                    nullBlocks.Add(new ShinyLocation(tempX, tempY));
                }
            }
        }

        public void generateLevel17()
        {
            //Introduction of gray blocks
            List<int> colors = generateRandomNumbers(3, 1, 4, true);
            generateRow(height - 1);
            generateRow(height - 2);
            generateRow(height - 3);
            generateRow(height - 4);

            //Make the "maze"
            for (int i = 0; i < 20; i++)
                blocks[i, height - 5] = 100;
            for (int i = 5; i < 20; i++)
                blocks[i, height - 12] = 100;
            for (int i = 1; i < 6; i++)
                blocks[0, height - i] = 100;
            for (int i = 1; i < 13; i++)
                blocks[5, height - i] = 100;
            for (int i = 1; i < 13; i++)
                blocks[11, height - i] = 100;
            for (int i = 1; i < 13; i++)
                blocks[17, height - i] = 100;

            blocks[5, height - 10] = 0;
            blocks[17, height - 10] = 0;
            blocks[19, height - 12] = 0;
            blocks[9, height - 6] = colors[0];
            blocks[10, height - 6] = colors[1];
            blocks[14, height - 6] = colors[2];
            shinies.Clear();
            shinies.Add(new ShinyLocation(9, height - 6));
            shinies.Add(new ShinyLocation(10, height - 6));
            shinies.Add(new ShinyLocation(14, height - 6));
        }

        public void generateLevel18()
        {
            List<int> colors = generateRandomNumbers(3, 1, 4, true);

            //The basic idea of this is that the bumps will cause the steps to
            //rise up over the gray block, allowing the shiny block that is not linked
            //to be accessible without having to clear all the blocks above it

            //Make the first stack
            blocks[1, height - 2] = 100;
            blocks[1, height - 3] = colors[0];
            blocks[0, height - 3] = random.Next(4) + 1;
            blocks[1, height - 4] = random.Next(4) + 1;
            blocks[2, height - 3] = 100;
            links[0, height - 3] = new LinkTarget(1, height - 4);
            links[1, height - 4] = new LinkTarget(0, height - 3);
            shinies.Add(new ShinyLocation(1, height - 3));

            //Make the middle puzzle
            int middleColor = colors[1];
            blocks[8, height - 3] = 100;
            blocks[9, height - 4] = 100;
            blocks[9, height - 3] = middleColor;
            shinies.Add(new ShinyLocation(9, height - 3));
            blocks[9, height - 2] = 100;
            blocks[10, height - 1] = middleColor;
            blocks[12, height - 1] = middleColor;
            for (int i = 2; i < 6; i++)
            {
                while (blocks[10, height - i] == 0 || blocks[10, height - i] == blocks[10, height - i - 1] || blocks[10, height - i] == middleColor)
                    blocks[10, height - i] = random.Next(4) + 1;
                while (blocks[12, height - i] == 0 || blocks[12, height - i] == blocks[12, height - i - 1] || blocks[12, height - i] == middleColor)
                    blocks[12, height - i] = random.Next(4) + 1;
            }
            blocks[13, height - 1] = 100;
            blocks[13, height - 2] = 100;
            blocks[13, height - 3] = 100;
            blocks[13, height - 4] = 100;


            //Make the third stack
            blocks[width - 2, height - 2] = 100;
            blocks[width - 2, height - 3] = colors[2];
            blocks[width - 1, height - 3] = random.Next(4) + 1;
            blocks[width - 2, height - 4] = random.Next(4) + 1;
            blocks[width - 3, height - 3] = 100;
            links[width - 1, height - 3] = new LinkTarget(width - 2, height - 4);
            links[width - 2, height - 4] = new LinkTarget(width - 1, height - 3);
            shinies.Add(new ShinyLocation(width - 2, height - 3));


            //Add blocks to the top of each stack
            for (int i = 5; i < 14; i++)
            {
                while (blocks[1, height - i] == 0 || blocks[1, height - i] == blocks[1, height - i + 1])
                    blocks[1, height - i] = random.Next(4) + 1;
                while (blocks[width - 2, height - i] == 0 || blocks[width - 2, height - i] == blocks[width - 2, height - i + 1])
                    blocks[width - 2, height - i] = random.Next(4) + 1;
            }
        }

        public void generateLevel19()
        {
            //Gray blocks all but two spaces on the death row, forcing the player to 
            //not move until past that point or face death
            for (int i = 0; i < 20; i++)
                if (i != 8 && i != 12)
                    blocks[i, height - 18] = 100;

            bumpTime = new TimeSpan(0, 0, 0); //No bumping in this level

            generateRow(height - 1);
            generateRow(height - 2);
            generateRow(height - 3);
            generateRow(height - 4);
            generateRow(height - 5);
            generateRow(height - 6);
            generateRow(height - 7);

            shinies.Clear();
            List<int> floatingColors = generateRandomNumbers(3, 1, 4, true);
            List<int> floatingTowerXs = generateRandomNumbers(3, 0, 19, true);
            for (int i = 0; i < 3; i++)
            {
                blocks[floatingTowerXs[i], height - 15] = floatingColors[i];
                blocks[floatingTowerXs[i], height - 14] = floatingColors[i];
                blocks[floatingTowerXs[i], height - 13] = floatingColors[i];
                locked[floatingTowerXs[i], height - 15] = true;
                locked[floatingTowerXs[i], height - 14] = true;
                locked[floatingTowerXs[i], height - 13] = true;
                shinies.Add(new ShinyLocation(floatingTowerXs[i], height - 15));
            }

            for (int i = 0; i < 20; i++)
            {
                if (i % 5 == 0)
                    drawBlocks.Add(new ShinyLocation(i, height - 8));
                else
                    nullBlocks.Add(new ShinyLocation(i, height - 8));
            }

        }

        public void generateLevel20()
        {
            //First lava level
            //All blocks are suspended, player must use existing platforms
            //And the suspend key to avoid hitting the ground
            //No bumps occurs in this level

            shinies.Clear();
            List<int> positions = generateRandomNumbers(3, 0, 19, true);
            List<int> colors = generateRandomNumbers(3, 1, 4, true);
            for (int i = 0; i < positions.Count; i++)
            {
                int x = positions[i];
                int color = colors[i];

                blocks[x, height - 2] = color;
                blocks[x, height - 4] = color;
                blocks[x, height - 5] = color;
                blocks[x, height - 6] = 100;
                locked[x, height - 2] = true;
                locked[x, height - 4] = true;
                locked[x, height - 5] = true;

                shinies.Add(new ShinyLocation(x, height - 4));
            }

            vortexMode = true;
            bumpTime = new TimeSpan(0, 0, 0);
        }

        public void generateLevel21()
        {
            //Another lava level, this one
            //has bunch of blocks supported by suspended linked blocks
            //Again, the player must avoid hitting the ground
            //This includes avoiding breaking the linked blocks and causing them to hit the ground
            //There should be an easy to get shiny that causes the bricks to fall to the ground, giving a game over
            //A different method for deleting it should be used (Or it should be saved for last)
            shinies.Clear();

            //Make the two mouse traps at the bottom
            List<int> colors = generateRandomNumbers(4, 1, 4, true);
            for (int x = 1; x < 17; x += 7)
            {
                int colorIndex = (x != 1 ? (x == 8 ? 1 : 2) : 0);
                int heightModifier = (x == 15 ? 1 : 0);
                blocks[x, height - 3 + heightModifier] = colors[colorIndex];
                blocks[x, height - 4 + heightModifier] = colors[colorIndex];
                blocks[x, height - 5 + heightModifier] = colors[colorIndex];
                blocks[x + 1, height - 3 + heightModifier] = random.Next(4) + 1;
                locked[x, height - 3 + heightModifier] = true;
                links[x, height - 3 + heightModifier] = new LinkTarget(x + 1, height - 3 + heightModifier);
                links[x + 1, height - 3 + heightModifier] = new LinkTarget(x, height - 3 + heightModifier);
                shinies.Add(new ShinyLocation(x, height - 5 + heightModifier));
            }

            //Make a simple falling puzzle
            blocks[13, height - 8] = 100;
            blocks[14, height - 7] = 100;
            blocks[15, height - 7] = 100;
            blocks[16, height - 7] = 100;
            blocks[13, height - 9] = 100;
            blocks[14, height - 10] = 100;
            blocks[15, height - 9] = 100;
            blocks[14, height - 8] = colors[3];
            blocks[15, height - 8] = colors[3];
            blocks[16, height - 8] = colors[3];
            blocks[14, height - 9] = colors[3];
            nullBlocks.Add(new ShinyLocation(14, height - 9));

            //Generate a random set of draw/null blocks
            List<int> randomX = generateRandomNumbers(9, 1, 10, true);
            List<int> randomY = generateRandomNumbers(6, 8, 13, true);
            for (int i = 0; i < 6; i++)
                drawBlocks.Add(new ShinyLocation(randomX[i], height - randomY[i]));
            for (int i = 0; i < 3; i++)
                drawBlocks.Add(new ShinyLocation(randomX[6 + i], height - randomY[i]));

            for (int x = 1; x < 11; x++)
            {
                for (int y = 8; y < 14; y++)
                {
                    //Make sure this isn't a draw block
                    bool add = true;
                    foreach (ShinyLocation sl in drawBlocks)
                        if (sl.x == x && sl.y == height - y)
                            add = false;
                    if(add)
                        nullBlocks.Add(new ShinyLocation(x, height - y));
                }
            }

            vortexMode = true;
            bumpTime = new TimeSpan(0, 0, 0);
        }

        public void generateLevel22()
        {
            //Do a puzzle yay! :D
            shinies.Clear();

            List<int> colors = generateRandomNumbers(4, 1, 4, true);
            int color1 = colors[0];
            int color2 = colors[1];
            int color3 = colors[2];
            int color4 = colors[3];

            //Create the double trap puzzle in the bottom left
            blocks[0, height - 2] = 100;
            blocks[2, height - 2] = color1;
            blocks[3, height - 2] = color1;
            blocks[4, height - 2] = color1;
            for (int i = 3; i < 12; i++)
                while (blocks[2, height - i] == 0 || blocks[2, height - i] == blocks[2, height - (i - 1)])
                    blocks[2, height - i] = random.Next(4) + 1;
            shinies.Add(new ShinyLocation(2, height - 2));
            for (int i = 3; i <= 6; i++)
                while (blocks[i, height - 10] == 0 || blocks[i, height - 10] == blocks[i - 1, height - 10] || blocks[i, height - 10] == color2)
                    blocks[i, height - 10] = random.Next(4) + 1;
            for (int i = 3; i <= 5; i++)
                while (blocks[i, height - 11] == 0 || blocks[i, height - 11] == blocks[i - 1, height - 11] || blocks[i, height - 11] == color2 || blocks[i, height - 11] == blocks[i, height - 10])
                    blocks[i, height - 11] = random.Next(4) + 1;
            blocks[6, height - 11] = color2;
            blocks[7, height - 10] = color2;
            blocks[8, height - 10] = color2;
            blocks[9, height - 10] = color2;
            locked[7, height - 10] = true;
            locked[8, height - 10] = true;
            locked[9, height - 10] = true;
            for (int i = 11; i <= 14; i++)
                while (blocks[7, height - i] == 0 || blocks[7, height - i] == blocks[7, height - (i - 1)] || blocks[7, height - i] == color3)
                    blocks[7, height - i] = random.Next(4) + 1;
            for (int i = 2; i <= 5; i++)
            {
                links[i, height - 11] = new LinkTarget(i + 1, height - 10);
                links[i + 1, height - 10] = new LinkTarget(i, height - 11);
            }
            while (blocks[7, height - 2] == 0 || blocks[7, height - 2] == color3)
                blocks[7, height - 2] = random.Next(4) + 1;
            blocks[7, height - 3] = color3;
            blocks[7, height - 4] = color3;
            blocks[7, height - 5] = color3;
            for (int x = 3; x <= 4; x++)
                for (int y = 4; y <= 6; y++)
                    blocks[x, height - y] = color1;
            for (int i = 4; i <= 5; i++)
                while (blocks[5, height - i] == 0 || blocks[5, height - i] == color1 || blocks[5, height - i] == blocks[5, height - (i - 1)])
                    blocks[5, height - i] = random.Next(4) + 1;
            for (int i = 4; i <= 5; i++)
                while (blocks[6, height - i] == 0 || blocks[6, height - i] == color3 || blocks[6, height - i] == blocks[6, height - (i - 1)])
                    blocks[6, height - i] = random.Next(4) + 1;
            links[7, height - 5] = new LinkTarget(6, height - 4);
            links[6, height - 4] = new LinkTarget(7, height - 5);
            for (int i = 3; i <= 5; i++)
            {
                links[i, height - 4] = new LinkTarget(i + 1, height - 5);
                links[i + 1, height - 5] = new LinkTarget(i, height - 4);
            }
            shinies.Add(new ShinyLocation(7, height - 5));
            blocks[8, height - 5] = 100;
            for (int i = 0; i <= 10; i++)
                blocks[i, height - 1] = 100;
            for (int i = 3; i <= 4; i++)
                while (blocks[i, height - 7] == 0 || blocks[i, height - 7] == color1 || blocks[i, height - 7] == blocks[i - 1, height - 7])
                    blocks[i, height - 7] = random.Next(4) + 1;
            for (int i = 3; i <= 4; i++)
                while (blocks[i, height - 8] == 0 || blocks[i, height - 8] == color1 || blocks[i, height - 8] == blocks[i - 1, height - 8] || blocks[i, height - 8] == blocks[i, height - 7])
                    blocks[i, height - 8] = random.Next(4) + 1;
            blocks[7, height - 9] = random.Next(4) + 1;
            links[7, height - 9] = new LinkTarget(8, height - 10);
            links[8, height - 10] = new LinkTarget(7, height - 9);
            blocks[8, height - 9] = color1;
            blocks[6, height - 8] = color1;
            blocks[7, height - 8] = color1;
            blocks[8, height - 7] = color2;
            blocks[9, height - 8] = color1;
            blocks[7, height - 7] = color2;
            links[9, height - 10] = new LinkTarget(8, height - 9);
            links[8, height - 9] = new LinkTarget(9, height - 10);
            links[6, height - 8] = new LinkTarget(7, height - 7);
            links[7, height - 7] = new LinkTarget(6, height - 8);
            locked[6, height - 8] = true;
            locked[7, height - 8] = true;
            locked[8, height - 7] = true;
            locked[9, height - 8] = true;

            //Generate three randomly placed blocks that are not the "most needed" block on the left-hand side
            List<int> xValues = generateRandomNumbers(3, 11, 19, true);
            for (int i = 0; i < 3; i++)
                shinies.Add(new ShinyLocation(xValues[i], height - 1));
            blocks[xValues[0], height - 1] = color1;
            blocks[xValues[1], height - 1] = color3;
            blocks[xValues[2], height - 1] = color4;
        }

        public void generateLevel23()
        {
            //Four areas walled off by gray blocks, with a mini puzzle in each one
            shinies.Clear();
            for (int i = 1; i < 19; i++)
            {
                blocks[4, height - i] = 100;
                blocks[9, height - i] = 100;
                blocks[14, height - i] = 100;
                blocks[19, height - i] = 100;
            }

            //Area 1 puzzle; The "bump solves it automatically" puzzle
            for (int i = 0; i < 4; i++)
                blocks[i, height - 5] = 100;
            int area1Color = random.Next(4) + 1;
            for(int i=0; i<=2; i++)
            {
                blocks[i, height - 1] = 100;
                blocks[i, height - 2] = area1Color;
            }
            shinies.Add(new ShinyLocation(0, height - 2));

            //Area 2 puzzle; cascading wall buster
            List<int> area2Colors = generateRandomNumbers(2, 1, 4, true);
            int area2Color1 = area2Colors[0];
            int area2Color2 = area2Colors[1];
            blocks[8, height - 1] = area2Color1;
            shinies.Add(new ShinyLocation(8, height - 1));
            for (int i = 3; i <= 5; i++)
            {
                blocks[5, height - i] = area2Color1;
                locked[5, height - i] = true;
            }
            for (int i = 6; i <= 8; i++)
            {
                blocks[i, height - 3] = area2Color2;
                locked[i, height - 3] = true;

                blocks[i, height - 9] = area2Color1;
                locked[i, height - 9] = true;
            }
            for (int i = 5; i <= 7; i++)
            {
                blocks[i, height - 6] = area2Color2;
                locked[i, height - 6] = true;
            }
            blocks[5, height - 7] = area2Color1;
            blocks[8, height - 10] = area2Color2;
            blocks[5, height - 8] = area2Color2;
            blocks[8, height - 5] = area2Color1;
            locked[8, height - 5] = true;
            blocks[5, height - 1] = area2Color1;
            blocks[8, height - 11] = area2Color1;
            blocks[8, height - 12] = area2Color1;
            blocks[8, height - 13] = area2Color1;
            blocks[5, height - 2] = area2Color2;
            blocks[6, height - 4] = area2Color1;
            blocks[7, height - 4] = area2Color1;

            //Area 3 puzzle; No landing zone
            for (int i = 10; i <= 13; i++)
                nullBlocks.Add(new ShinyLocation(i, height - 1));
            drawBlocks.Add(new ShinyLocation(11, height - 5));
            drawBlocks.Add(new ShinyLocation(13, height - 7));

            //Area 4 puzzle; Easy clears with draw blocks that must be refilled
            List<int> area4Colors = generateRandomNumbers(4, 1, 4, true);
            for (int i = 15; i < 19; i++)
            {
                blocks[i, height - 1] = area4Colors[i - 15];
                blocks[i, height - 2] = area4Colors[i - 15];
                blocks[i, height - 3] = area4Colors[i - 15];
                shinies.Add(new ShinyLocation(i, height - 3));
                drawBlocks.Add(new ShinyLocation(i, height - 1));
            }
        }

        public void generateLevel24()
        {
            //Use Timer Control to cause the player to have to survive a certain amount of time (see Page 57)
            shinies.Clear();
            for (int i = 0; i < 3; i++)
                blocks[i, height - 1] = 100;
            for (int i = 1; i < 11; i++)
                blocks[4, height - i] = 100;
            for (int i = 0; i < 4; i++)
                blocks[i, height - 10] = 100;
            int color = random.Next(4) + 1;
            blocks[3, height - 1] = color;
            blocks[0, height - 9] = color;
            blocks[1, height - 9] = color;
            blocks[2, height - 9] = color;
            locked[0, height - 9] = true;
            locked[1, height - 9] = true;
            locked[2, height - 9] = true;
            shinies.Add(new ShinyLocation(0, height - 9));

            //Make a set that needs a spun block to clear:
            for (int i = 12; i < 21; i++)
                blocks[4, height - i] = 100;
            blocks[0, height - 12] = color;
            blocks[1, height - 12] = color;
            blocks[2, height - 12] = color;
            locked[0, height - 12] = true;
            locked[1, height - 12] = true;
            locked[2, height - 12] = true;
            shinies.Add(new ShinyLocation(2, height - 12));

            //Generate random rows without auto deletes
            for(int i=1; i<9; i++)
                generateRow(height - i, 5, width, true);

            //Generate the clean sweeps
            List<int> colors = generateRandomNumbers(4, 1, 4, true);
            int color1 = colors[0];
            int color2 = colors[1];
            int color3 = colors[2];
            int color4 = colors[3];
            for (int i = 5; i < 8; i++)
            {
                blocks[i, height - 11] = color1;
                locked[i, height - 11] = true;
            }
            for (int i = 9; i < 12; i++)
            {
                blocks[i, height - 11] = color2;
                locked[i, height - 11] = true;
            }
            for (int i = 13; i < 16; i++)
            {
                blocks[i, height - 11] = color3;
                locked[i, height - 11] = true;
            }
            for (int i = 17; i < 20; i++)
            {
                blocks[i, height - 11] = color4;
                locked[i, height - 11] = true;
            }
            for (int i = 5; i < 7; i++)
            {
                blocks[i, height - 10] = color4;
                locked[i, height - 10] = true;
            }
            for (int i = 8; i < 11; i++)
            {
                blocks[i, height - 10] = color4;
                locked[i, height - 10] = true;
            }
            for (int i = 12; i < 15; i++)
            {
                blocks[i, height - 10] = color1;
                locked[i, height - 10] = true;
            }
            for (int i = 16; i < 19; i++)
            {
                blocks[i, height - 10] = color1;
                locked[i, height - 10] = true;
            }
            for (int i = 6; i < 9; i++)
            {
                blocks[i, height - 9] = color2;
                locked[i, height - 9] = true;
            }
            for (int i = 10; i < 13; i++)
            {
                blocks[i, height - 9] = color2;
                locked[i, height - 9] = true;
            }
            for (int i = 14; i < 17; i++)
            {
                blocks[i, height - 9] = color3;
                locked[i, height - 9] = true;
            }
            blocks[18, height - 9] = color3;
            locked[18, height - 9] = true;

            bumpTime = new TimeSpan(0, 0, 0, 0, 20000);
        }

        public void generateLevel25()
        {
            //Entire playing field split by gray blocks
            //The right side has no shinies but requires the right block to dodge through a tunnel
            //to avoid going over the line
            //No bumps on this one
            //Left side will not be complicated, the difficulty will be in coordinating movement from the right side
            shinies.Clear();

            //Create the line of gray blocks in the middle
            for (int y = 0; y < height; y++)
                blocks[10, y] = 100;

            #region Generate the maze on the right side

            //Create all the starting blocks
            for (int i = 2; i < height; i++)
                generateRow(i, 11, width, true);
            for (int x = 11; x < width; x++)
                for (int y = 2; y < height; y++)
                    locked[x, y] = true;

            //Cut small holes on the right side
            List<int> holeLocations = generateRandomNumbers(4, 11, width - 1, true);
            int holeOne = holeLocations[0];
            int holeTwo = holeLocations[1];
            int holeThree = holeLocations[2];
            int holeFour = holeLocations[3];
            List<int> directionSwitches = generateRandomNumbers(2, 5, height - 2, true);
            int switchOne = (directionSwitches[0] < directionSwitches[1] ? directionSwitches[0] : directionSwitches[1]);
            int switchTwo = (directionSwitches[0] > directionSwitches[1] ? directionSwitches[0] : directionSwitches[1]);
            if (switchOne == switchTwo - 1)
                switchOne--;
            else if (switchOne - 1 == switchTwo)
                switchTwo--;

            for (int i = 2; i < 6; i++)
            {
                blocks[holeOne, i] = 0;
                locked[holeOne, i] = false;
            }

            for (int i = 2; i < 8; i++)
            {
                blocks[holeTwo, i] = 0;
                locked[holeTwo, i] = false;
            }

            int currentX = holeThree;
            for (int i = 2; i < height; i++)
            {
                if (i == switchOne)
                {
                    if (currentX <= 15)
                    {
                        for (int x = currentX; x < currentX + 4; x++)
                        {
                            blocks[x, i] = 0;
                            locked[x, i] = false;
                        }
                        currentX = currentX + 3;
                    }
                    else
                    {
                        for (int x = currentX; x > currentX - 4; x--)
                        {
                            blocks[x, i] = 0;
                            locked[x, i] = false;
                        }
                        currentX = currentX - 3;
                    }
                }
                else if (i == switchTwo)
                {
                    if (currentX <= 15)
                    {
                        for (int x = currentX; x < currentX + 4; x++)
                        {
                            blocks[x, i] = 0;
                            locked[x, i] = false;
                        }
                        currentX = currentX + 3;
                    }
                    else
                    {
                        for (int x = currentX; x > currentX - 4; x--)
                        {
                            blocks[x, i] = 0;
                            locked[x, i] = false;
                        }
                        currentX = currentX - 3;
                    }
                }
                else
                {
                    blocks[currentX, i] = 0;
                    locked[currentX, i] = false;
                }
            }

            for (int i = 2; i < height - 14; i++)
            {
                if (i <= height - 7)
                {
                    blocks[holeFour, i] = 0;
                    locked[holeFour, i] = false;
                }
                else
                {
                    if (holeFour == 11)
                    {
                        blocks[holeFour, i] = 0;
                        locked[holeFour, i] = false;
                        blocks[holeFour + 1, i] = 0;
                        locked[holeFour + 1, i] = false;
                    }
                    else
                    {
                        blocks[holeFour, i] = 0;
                        locked[holeFour, i] = false;
                        blocks[holeFour - 1, i] = 0;
                        locked[holeFour - 1, i] = false;
                    }
                }
            }
            #endregion 

            #region Generate the puzzle on the left side
            int puzzle = random.Next(3);

            if (puzzle == 0)
            {
                #region Puzzle One
                //Towers with bridges between them
                List<int> bridges = generateRandomNumbers(2, 5, height - 5, true);
                foreach (int i in bridges)
                {
                    generateRow(i, 0, 9, true);
                    for (int x = 0; x <= 10; x++)
                        locked[x, i] = true;
                }
                List<int> towers = generateRandomNumbers(3, 0, 9, true);
                foreach (int i in towers)
                    generateQuickKillColorTower(i, 4);
                foreach (int i in towers)
                    shinies.Add(new ShinyLocation(i, height - 2));
                #endregion
            }
            else if (puzzle == 1)
            {
                #region Puzzle Two
                //Randomly Generated draw/null blocks strewn about
                List<int> rowTypes = generateRandomNumbers(6, 0, 5, true);
                for (int i = 0; i < rowTypes.Count; i++)
                {
                    int y = (i * 2) + 5;
                    if (rowTypes[i] == 0)
                    {
                        //Generate Three Gray Blocks with Draws, and one with nulls
                        List<int> xValues = generateRandomNumbers(4, 0, 9, true);

                        for (int x = 0; x < 3; x++)
                        {
                            blocks[xValues[x], y + 1] = 100;
                            drawBlocks.Add(new ShinyLocation(xValues[x], y));
                        }

                        blocks[xValues[3], y + 1] = 100;
                        nullBlocks.Add(new ShinyLocation(xValues[3], y));
                    }
                    else if (rowTypes[i] == 1)
                    {
                        //Generate five random null blocks
                        List<int> xValues = generateRandomNumbers(5, 0, 9, true);
                        for (int x = 0; x < 5; x++)
                        {
                            blocks[xValues[x], y + 1] = 100;
                            nullBlocks.Add(new ShinyLocation(xValues[x], y));
                        }
                    }
                    else if (rowTypes[i] == 2)
                    {
                        //Generate three floating draw blocks
                        List<int> xValues = generateRandomNumbers(3, 0, 9, true);
                        for (int x = 0; x < xValues.Count; x++)
                            drawBlocks.Add(new ShinyLocation(xValues[x], y + 1));
                    }
                    else if (rowTypes[i] == 3)
                    {
                        //Generate one horizontal row with null blocks and three of one color
                        List<int> xValues = generateRandomNumbers(1, 0, 4, true);
                        for (int x = xValues[0]; x < xValues[0] + 6; x++)
                        {
                            blocks[x, y + 1] = 100;
                            nullBlocks.Add(new ShinyLocation(x, y));
                        }
                        int color = random.Next(4) + 1;
                        for (int x = xValues[0]; x < xValues[0] + 3; x++)
                            blocks[x, y] = color;
                    }
                    else if (rowTypes[i] == 4)
                    {
                        //Generate three different colors towers that are floating and have shinies in them
                        int towerX1 = 13;
                        while (towerX1 == 13 || blocks[towerX1, y - 1] == 100)
                            towerX1 = random.Next(10) + 1;

                        int towerX2 = 13;
                        while (towerX2 == 13 || blocks[towerX2, y - 1] == 100 || towerX1 == towerX2)
                            towerX2 = random.Next(10) + 1;

                        int towerX3 = 13;
                        while (towerX3 == 13 || blocks[towerX3, y - 1] == 100 || towerX1 == towerX3 || towerX2 == towerX3)
                            towerX3 = random.Next(10) + 1;

                        List<int> colors = generateRandomNumbers(3, 1, 4, true);

                        for (int currentY = y - 1; currentY <= y + 1; currentY++)
                        {
                            blocks[towerX1, currentY] = colors[0];
                            locked[towerX1, currentY] = true;
                        }
                        for (int currentY = y - 1; currentY <= y + 1; currentY++)
                        {
                            blocks[towerX2, currentY] = colors[1];
                            locked[towerX2, currentY] = true;
                        }
                        for (int currentY = y - 1; currentY <= y + 1; currentY++)
                        {
                            blocks[towerX3, currentY] = colors[2];
                            locked[towerX3, currentY] = true;
                        }

                        shinies.Add(new ShinyLocation(towerX1, y));
                        shinies.Add(new ShinyLocation(towerX2, y));
                        shinies.Add(new ShinyLocation(towerX3, y));
                    }
                }
                #endregion
            }
            else
            {
                #region Puzzle Three
                //Linked blocks puzzle
                List<int> colors = generateRandomNumbers(4, 1, 4, true);
                int currentColor = 0;

                //Create two rows of mini trees
                for (int i = 0; i < 2; i++)
                {
                    int y = 3 + (i * 5);
                    int displace = random.Next(2);
                    List<int> sets = generateRandomNumbers((i == 0 ? 2 : 3), 0, 2, true);
                    foreach (int j in sets)
                    {
                        int x = displace + (j * 3);
                        blocks[x + 2, y] = colors[currentColor % 4];
                        currentColor++;
                        blocks[x, y + 1] = colors[currentColor % 4];
                        currentColor++;
                        blocks[x + 2, y + 2] = colors[currentColor % 4];
                        currentColor++;
                        blocks[x + 1, y + 1] = colors[currentColor % 4];
                        blocks[x + 1, y + 2] = colors[currentColor % 4];
                        blocks[x + 1, y + 3] = colors[currentColor % 4];
                        blocks[x + 1, y + 4] = 100;
                        currentColor++;
                        currentColor++;
                        links[x + 2, y] = new LinkTarget(x + 1, y + 1);
                        links[x + 1, y + 1] = new LinkTarget(x + 2, y);
                        links[x, y + 1] = new LinkTarget(x + 1, y + 2);
                        links[x + 1, y + 2] = new LinkTarget(x, y + 1);
                        links[x + 1, y + 3] = new LinkTarget(x + 2, y + 2);
                        links[x + 2, y + 2] = new LinkTarget(x + 1, y + 3);
                        shinies.Add(new ShinyLocation(x + 1, y + 2));
                    }
                }

                //Create the link puzzle at the bottom
                for (int i = 3; i < 6; i++)
                    blocks[0, height - i] = colors[currentColor % 4];
                currentColor++;
                blocks[0, height - 2] = 100;
                links[0, height - 3] = new LinkTarget(1, height - 3);
                links[1, height - 3] = new LinkTarget(0, height - 3);

                for (int i = 1; i < 9; i++)
                {
                    blocks[i, height - 4] = colors[currentColor % 4];
                    blocks[i, height - 3] = colors[currentColor % 4];
                    blocks[i, height - 1] = colors[currentColor % 4];
                    if (i != 8)
                    {
                        links[i, height - 4] = new LinkTarget(i + 1, height - 3);
                        links[i + 1, height - 3] = new LinkTarget(i, height - 4);
                    }
                    currentColor++;
                }

                for (int i = 3; i < 6; i++)
                    shinies.Add(new ShinyLocation(i, height - 4));
                #endregion
            }


            #endregion

            bumpTime = new TimeSpan(0, 0, 0);
        }

        public bool hasVictory()
        {
            if (shinies.Count == 0 && drawBlocksCleared() && nullBlocksCleared())
                return true;
            return false;
        }

        public bool nullBlocksCleared()
        {
            foreach (ShinyLocation sl in nullBlocks)
                if (blocks[sl.x, sl.y] != 0)
                    return false;

            return true;
        }

        public bool drawBlocksCleared()
        {
            foreach (ShinyLocation sl in drawBlocks)
                if(blocks[sl.x, sl.y] == 0)
                    return false;

            return true;
        }

        public void clearLevel()
        {
            blocks = new int[width, height];
            links = new LinkTarget[width, height];
            locked = new bool[width, height];
            deleteTimers = new int[width, height];
            startX1 = 8;
            startX2 = 12;
            bumpTime = new TimeSpan(0, 0, 10); //Set to null to stop increase from occurring
            dropTime = new TimeSpan(0, 0, 0, 0, 1000);
            shinies = new List<ShinyLocation>();
        }

        public void nextLevel()
        {
            currentLevel++;
            clearLevel();
            loadLevel(currentLevel);
        }

        public bool isEndGame()
        {
            return (currentLevel == 25 && hasVictory());
        }

        #region Bad (or overly complicated and buggy) methods
        private void generateTree(int startX, int startY, int height, int trunkSize, int branchLength, bool useSameBranchColor = false, int trunkStripe = 0, int branchGap = 2, int branchShinies = 0)
        {
            //Trunk Size:  Width of trunk
            //Branch Length: How long each branch is (at most)
            //useSameBranchColor: Set to true to have all branches be the same color
            //TrunkStripe:  0 to ignore, 1+ to have "stripes" on the tree of height trunkStripe
            //branchGap: How many spaces (in height) before branches are redrawn
            //BranchShinies: How many shinies to include in the branches

            //Generate a tree using links

            //Calculate branch location of shinies
            int branchCount = 0;
            int maxBranches = ((height - 1) / branchGap) * (trunkSize == 1 ? 1 : 2);
            List<int> shinyCountLocation = new List<int>();
            if (branchShinies != 0)
                shinyCountLocation = generateRandomNumbers(branchShinies, 0, maxBranches, true);

            int branchGapCount = branchGap - 1;
            int trunkStripeCount = 0;
            int currentTrunkColorIndex = 0;
            List<int> trunkColors = generateRandomNumbers(3, 1, 4, true);
            int branchColor = trunkColors[2];


            for (int i = 0; i < height; i++)
            {
                //Determine if this is a branch level or not
                if (branchGapCount == branchGap)
                {
                    //Branch layer
                    if (!useSameBranchColor)
                        branchColor = random.Next(4) + 1;
                    if (trunkSize == 1)
                    {
                        #region Only one, generate only a branch
                        int tempBranchLength = random.Next(branchLength + 1);
                        int branchDirection = random.Next(2); //Either a positive or negative vertical branch direction
                        if (branchDirection == 0)
                            branchDirection = -1;
                        int branchHorizontalDirection = random.Next(); //Branch out in a random horizontal direction
                        if (branchHorizontalDirection == 0)
                            branchHorizontalDirection = -1;
                        bool makeShiny = false;
                        if (shinyCountLocation.Contains(branchCount))
                            makeShiny = true;
                        for (int n = 0; n < tempBranchLength; n++)
                        {
                            blocks[startX + (n * branchHorizontalDirection), startY - i + (n * branchDirection)] = branchColor;
                            blocks[startX + 1 + (n * branchHorizontalDirection), startY - i + (n * branchDirection) + branchDirection] = branchColor;
                            links[startX + (n * branchHorizontalDirection), startY - i + (n * branchDirection)] = new LinkTarget(startX + 1 + (n * branchHorizontalDirection), startY - i + (n * branchDirection) + branchDirection);
                            links[startX + 1 + (n * branchHorizontalDirection), startY - i + (n * branchDirection) + branchDirection] = new LinkTarget(startX + (n * branchHorizontalDirection), startY - i + (n * branchDirection));
                            if (makeShiny)
                                shinies.Add(new ShinyLocation(startX + 1 + (n * branchHorizontalDirection), startY - i + (n * branchDirection) + branchDirection));
                        }

                        branchCount++;
                        #endregion
                    }
                    else
                    {
                        //Move than one width, generate a branch on both sides

                        #region Generate Left Branch
                        int tempBranchLength = random.Next(branchLength + 1);
                        int branchDirection = random.Next(2); //Either a positive or negative vertical branch direction
                        if (branchDirection == 0)
                            branchDirection = -1;
                        int branchHorizontalDirection = -1; //Branch Left
                        bool makeShiny = false;
                        if (shinyCountLocation.Contains(branchCount))
                            makeShiny = true;
                        for (int n = 0; n < tempBranchLength; n++)
                        {
                            blocks[startX + (n * branchHorizontalDirection), startY - i + (n * branchDirection)] = branchColor;
                            blocks[startX + 1 + (n * branchHorizontalDirection), startY - i + (n * branchDirection) + branchDirection] = branchColor;
                            links[startX + (n * branchHorizontalDirection), startY - i + (n * branchDirection)] = new LinkTarget(startX + 1 + (n * branchHorizontalDirection), startY - i + (n * branchDirection) + branchDirection);
                            links[startX + 1 + (n * branchHorizontalDirection), startY - i + (n * branchDirection) + branchDirection] = new LinkTarget(startX + (n * branchHorizontalDirection), startY - i + (n * branchDirection));
                            if (makeShiny)
                                shinies.Add(new ShinyLocation(startX + 1 + (n * branchHorizontalDirection), startY - i + (n * branchDirection) + branchDirection));
                        }

                        branchCount++;
                        #endregion

                        #region Generate Right Branch
                        tempBranchLength = random.Next(branchLength + 1);
                        branchDirection = random.Next(2); //Either a positive or negative vertical branch direction
                        if (branchDirection == 0)
                            branchDirection = -1;
                        branchHorizontalDirection = 1; //Branch Left
                        makeShiny = false;
                        if (shinyCountLocation.Contains(branchCount))
                            makeShiny = true;
                        for (int n = 0; n < tempBranchLength; n++)
                        {
                            blocks[startX + (trunkSize - 1) + (n * branchHorizontalDirection), startY - i + (n * branchDirection)] = branchColor;
                            blocks[startX + (trunkSize - 1) + 1 + (n * branchHorizontalDirection), startY - i + (n * branchDirection) + branchDirection] = branchColor;
                            links[startX + (trunkSize - 1) + (n * branchHorizontalDirection), startY - i + (n * branchDirection)] = new LinkTarget(startX + (trunkSize - 1) + 1 + (n * branchHorizontalDirection), startY - i + (n * branchDirection) + branchDirection);
                            links[startX + (trunkSize - 1) + 1 + (n * branchHorizontalDirection), startY - i + (n * branchDirection) + branchDirection] = new LinkTarget(startX + (trunkSize - 1) + (n * branchHorizontalDirection), startY - i + (n * branchDirection));
                            if (makeShiny)
                                shinies.Add(new ShinyLocation(startX + 1 + (n * branchHorizontalDirection), startY - i + (n * branchDirection) + branchDirection));
                        }

                        branchCount++;
                        #endregion

                        #region Generate Trunk
                        int horizontalTrunkColorCount = 1;
                        for (int n = 1; n < trunkSize - 1; n++)
                        {
                            int currentTrunkColorModifier = currentTrunkColorIndex;
                            horizontalTrunkColorCount++;
                            if (horizontalTrunkColorCount == 3)
                            {
                                currentTrunkColorModifier = (currentTrunkColorModifier == 0 ? 1 : 0);
                                horizontalTrunkColorCount = 0;
                            }
                            blocks[startX + n, startY - i] = trunkColors[currentTrunkColorModifier];
                        }
                        #endregion

                        branchGapCount = -1; //Will be incremented at end to 0
                    }
                }
                else
                {
                    //Trunk Only layer
                    int horizontalTrunkColorCount = 0;
                    for (int n = 0; n < trunkSize; n++)
                    {
                        int currentTrunkColorModifier = currentTrunkColorIndex;
                        horizontalTrunkColorCount++;
                        if (horizontalTrunkColorCount == 3)
                        {
                            currentTrunkColorModifier = (currentTrunkColorModifier == 0 ? 1 : 0);
                            horizontalTrunkColorCount = 0;
                        }
                        blocks[startX + n, startY - i] = trunkColors[currentTrunkColorModifier];
                    }
                }

                //increment counters
                branchGapCount++;
                trunkStripeCount++;

                //Increment trunk current color if necessary
                if (trunkStripeCount == trunkStripe)
                {
                    currentTrunkColorIndex = (currentTrunkColorIndex == 1 ? 0 : 1);
                    trunkStripeCount = 0;
                }
            }
        }

        #endregion
    }

    public class BlockClearAnimation
    {
        public Texture2D clear1;
        public Texture2D clear2;
        public Texture2D clear3;

        public BlockClearAnimation(Texture2D clear1, Texture2D clear2, Texture2D clear3)
        {
            this.clear1 = clear1;
            this.clear2 = clear2;
            this.clear3 = clear3;
        }
    }

    public class ShinyLocation
    {
        public int x;
        public int y;
        public ShinyLocation(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
