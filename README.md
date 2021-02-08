Unity-Visual-novel-tools
This is a repository of all the usual tools and files I use when building a visual novel in Unity
my intention is to keep the original version of all these tools here so i can copy and change them as needed for different games

includes:

*CSVREADER.cs, scripts written in google spreadsheets can be converted to csvs, this program extracts all the relevant information as a list of dictionary objects

*Dialogue.cs, the basic structure for storing all the relevant info from the story script to be used in game 

*a quickly thrown together demo of one basic way to implement and modify the scripts in unity 

TODO: at some point I wantto go back and reimplement the data structure that contains all the dialouge options to be a stack or queue instead of a dictionary, the original idea of implementing the idctionary was so that you could potentially go backwards in the script, and the fact that this tool was built under time constraints of a game jam meant that we had to work fast and build a solution on the fly. however I havent found that I usually end up going backwards through dialouge in visula novels, and having the entire script stored throughout the entire game as objects could be space intense if the script text is to long, and that was just never a problem we had to consider when working with the smaller script texts of a game jam, and this problem could potentially be solved by using a stack or queue, and throughing out dialouge as it is no longer needed. 

examples of use of tools:

https://theavianlord.itch.io/lifeline
https://github.com/timkashani/souptime
https://github.com/shoshanimayan/Interviews-with-my-friends-AR
