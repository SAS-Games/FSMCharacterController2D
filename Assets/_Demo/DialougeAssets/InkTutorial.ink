INCLUDE  globals.ink
I am your Instructor {instructor_name}
-> knot_example

== knot_example ==
[\==] This is a knot. A knot is a named container of story content. It allows you to organize your story into manageable sections.
To define a knot, you use the `==` symbols followed by the knot's name, like this: == Knot Example ==
-> divert_example

== divert_example ==
[\->] This is a divert. It allows us to jump to other parts of the story. Diverts are used to direct the flow of the narrative, guiding the reader to different sections based on choices or events.
To use a divert, you write the `\->` symbol followed by the name of the knot or stitch you want to jump to, like this:

Now we will demonstrate choices.
-> choice_example

== choice_example ==
Here are some choices:

 * [Normal Choice 1]
    -> normal_choice_1

 * [Normal Choice 2]
    -> normal_choice_2

 + [Sticky Choice 1]
    -> sticky_choice_1

 + [Sticky Choice 2]
    -> sticky_choice_2

-> END

== normal_choice_1 ==
This is a normal choice. Selecting it will direct you back to this section, allowing you to make another choice.
To define a normal choice, you use the `*` symbol followed by the choice text, like this: <b><color=red><size=30>*Choice_Text</size></color></b>
-> choice_example

== normal_choice_2 ==
This is another normal choice. Like the first, it will direct you back to this section.
To define a normal choice, you use the `*` symbol followed by the choice text, like this: <b><color=red><size=30>*Choice_Text</size></color></b>
-> choice_example

== sticky_choice_1 ==
This is a sticky choice. Sticky choices remain available even after they are selected, providing persistent options to the reader. Once selected, sticky choices will continue to be available throughout the story, allowing readers to revisit the choice if desired.
To define a sticky choice, you use the `+` symbol followed by the choice text, like this: <b><color=red><size=30>+Choice_Text</size></color></b>
-> choice_example

== sticky_choice_2 ==
This is another sticky choice. Like the first, sticky choices persist, allowing you to revisit them.
To define a sticky choice, you use the `+` symbol followed by the choice text, like this: <b><color=red><size=30>+Choice_Text</size></color></b>
-> END


