INCLUDE globals.ink
->introduction
== introduction ==
Welcome to this demo on how to use Inklet for creating a dialogue system in Unity with Ink.Press <b>Space Bar</b> to continue.
Inklet is an extension that makes writing and managing dialogues much easier, allowing you to create interactive stories and dialogues within Unity. 
This demo will guide you through the basic setup and integration process, showing you how to create dialogues, compile them, and use them in Unity with player choices. Let’s get started! 

{instructor_name == "": -> selectInstructor | ->already_choosen}

->selectInstructor

==selectInstructor==
Select your Instructor

+[Abilash]
    ->instructor("Abilash")
+[Abhishek]
    ->instructor("Abhishek")
+[Rajesh]
    ->instructor("Rajesh")
+[Saurabh]
    ->instructor("Saurabh")

== instructor(name) ==
~instructor_name = name
Your Instructor is {instructor_name}!
    -> END
== already_choosen ==
Your Instructor is {instructor_name}!
->END
