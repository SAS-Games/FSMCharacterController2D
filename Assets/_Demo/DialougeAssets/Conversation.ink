-> Alex_St_1

    == Alex_St_1 ==
     Hey Jamie, I've been working on our game project, and I heard about this tool called Inklet that can help with the dialogue system using Ink. Do you know anything about it? #speaker:tag::Alex, name::Alex, anim::Active,#speaker: tag::Jamie, name::Jamie, anim::Passive
-> Jamie_St_1
     == Jamie_St_1 ==
    Yeah, I've used Inklet before. It's a great extension for Ink in Unity. It makes writing and managing dialogues much easier. Want me to walk you through how to set it up and use it? #speaker: tag::Alex, anim::Passive #speaker: tag::Jamie, anim::Active
    *[Sure, that would be awesome!] -> sure
    *[No, I want to try figuring it out myself.] -> no_help
    == sure ==
    Sure, that would be awesome! I know how to use Ink for writing dialogues, but I'm not sure how to integrate it with Unity.#speaker: tag::Alex, anim::Active #speaker: tag::Jamie, anim::Passive
    No problem. First things first, have you installed the Ink Unity Integration package?#speaker: tag::Alex, anim::Active #speaker: tag::Jamie, anim::Passive
    
    * [Yes, I have that set up already.] -> yes_integration
    * [No, I haven't.] -> no_integration

== no_help ==
    No, I want to try figuring it out myself.#speaker: tag::Alex, anim::Active #speaker: tag::Jamie, anim::Passive
    Alright, let me know if you need any help later.#speaker: tag::Alex, anim::Passive #speaker: tag::Jamie, anim::Active
-> END
== yes_integration ==
    Yes, I have that set up already.#speaker: tag::Alex, anim::Active #speaker: tag::Jamie, anim::Passive
    Great! Now, let's get Inklet. It's an extension that you can find in the Unity Asset Store. Just download and import it into your project.#speaker: tag::Alex, anim::Passive #speaker: tag::Jamie, anim::Active
->END
== no_integration ==
    No, I haven't.#speaker: tag::Alex, anim::Active #speaker: tag::Jamie, anim::Passive
    Okay, you can find the Ink Unity Integration package on the Asset Store. Once you have it, import it into your project. Then, let's continue from there..#speaker: tag::Alex, anim::Passive #speaker: tag::Jamie, anim::Active
-> END