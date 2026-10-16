from museum import data_json, data_spatial_json

init_prompt = f"""You are a helpful tour guide that help people visit a virtual museum in virtual reality. The ultimate goal is to make people's visiting experiences more customized and natural.

This museum has stored some paintings and  their names, spatial positions and orientations (both stored in Unity 3D coordinate format) are shown below:
Space: {data_spatial_json}
"""

task_classify_prompt = """
I have a classification task about interactive types in virtual reality with four labels: "information enhancement", "navigation", "preference specification", and "error". For each input, you need to detect which kinds of interactive tasks are in virtual reality and select one or more labels as the output. Let's think step by step. 

"information enhancement" refers to questions that require information users are not familiar with to enhance their understanding of the virtual environment. Or the input is similar to "please explain more". 
"preference specification" refers to descriptions related to users themselves, like background and personal interest.
"navigation" means you want to move/go to another place or visit the whole virtual environment. You might say: "take me somewhere" or "give me a tour". 
If the input is not related to the current items of this environment, it will be defined as the "error" label. 

You should only respond the label(s) split by the comma without any other explanation or words. 

Here are examples: 
INPUT: 
Please give me a tour to visit this virtual museum.
RESPONSE: 
information enhancement

Here are examples: 
INPUT: 
Please help me plan a tour in 30 minutes.
RESPONSE: 
information enhancement

INPUT: 
Please guide me to the three most popular items.
RESPONSE: 
navigation, information enhancement

INPUT: 
Take me to visit the three most popular items.
RESPONSE: 
navigation, information enhancement

INPUT: 
I want to see "Mona Lisa".
RESPONSE: 
navigation

INPUT:
I am a chinese art teacher.
RESPONSE:
preference specification

INPUT:
Next
RESPONSE:
navigation

INPUT:
I want to see some modern style paintings, could you?
RESPONSE:
navigation, information enhancement, preference specification

INPUT:
What is the result for one plus one.
RESPONSE:
error

INPUT:
I'm a professor majoring in Chinese art history. Please recommend me a tour.
RESPONSE:
navigation, preference specification

INPUT:
"""

preference_prompt = """

"""

# extract contextual info for display
extract_prompt = """
1. detect if the user start visiting, during the tour, or finishing the tour.
2. If the user is at the beginning or ending of the tour, extract the context in 2-3 sentences and sometime you can use a clear way to display the information below directly such as bullet points if neccessary.
3. Otherwise, extract the important points with words or one-sentence description.
4. ONLY give the RESPONSE.

Here are several examples:
INPUT:
The Great Wave off Kanagawa is a woodblock print by Japanese ukiyo-e artist Hokusai, created in late 1831 during the Edo period of Japanese history. The print depicts three boats moving through a storm-tossed sea, with a large wave forming a spiral in the centre and Mount Fuji visible in the background.
RESPONSE:
Name: The Great Wave off Kanagawa
Author: Hokusai
Year: 1831
Country: Japan
Introduction: The print depicts three boats moving through a storm-tossed sea, with a large wave forming a spiral in the centre and Mount Fuji visible in the background.

INPUT:
I'm glad you're enjoying your visit to "Composition no.10" by Piet Mondrian. This painting, created in the early 1930s, is a significant example of the De Stijl movement, known for its use of geometric shapes and primary colors. If you're interested in exploring portrait paintings, I recommend checking out "Shen Zhou self portrait at age 80" by Shen Zhou and "Chardin pastel selfportrait" by Jean-Baptiste-Siméon Chardin.
RESPONSE:
Recommendation for the tour:
- Shen Zhou self portrait at age 80
- Chardin pastel selfportrait

INPUT:
Yes, there are several paintings related to Chinese culture in the museum. Here are some of them:
1. "Section of Goddess of Luo River" by Zhang Zeduan
   - Author: Zhang Zeduan
   - Country: China
   - Style: Chinese Song Dynasty
2. "Travelers among Mountains and Streams" by Fan Kuan
   - Author: Fan Kuan
   - Country: China
   - Style: Chinese Northern Song
3. "A Man and His Horse in the Wind" by Fu Baoshi
   - Author: Fu Baoshi
   - Country: China
   - Style: Modern Chinese Ink Painting
   - Position: [17.75, 2.57, 17.32]
4. "Forest Grotto at Juqu" by Dong Yuan
   - Author: Dong Yuan
   - Country: China
   - Style: Chinese Southern School
5. "Shen Zhou self portrait at age 80" by Shen Zhou
   - Author: Shen Zhou
   - Country: China
   - Style: Ming Dynasty Painting
These paintings showcase different styles and periods of Chinese art.
RESPONSE:
Recommendation for the tour:
- Section of Goddess of Luo River
- Travelers among Mountains and Streams
- A Man and His Horse in the Wind
- Forest Grotto at Juqu
- Shen Zhou self portrait at age 80

INPUT:
"""

info_prompt = """
You are a helpful tour guide that help people visit a virtual museum in virtual reality. The ultimate goal is to make people's visiting experiences more customized and natural.

You need to give the answers following the steps:
1. detect if the user start visiting, during the tour, or finishing the tour.
2. If the user is at the beginning or ending of the tour, give the general description and suggestions for overview or summarization respectively. Response in less than 5 sentences. 
3. During the progress, if the user is only interested in the current painting,  only give the most imporatant details about this painting like author, year, and style. Response in less than 3 sentences. 
4. During the progress, if the user relate to or compare different paintings. List all the related paintings name and one-sentence bio in a direct and clear way.
4. Do not mention anything mathmatical.

Here are some examples:
INPUT:
Now I am looking at the painting "The Great Wave". Please introduce me this painting.
Reason: The user is during the tour, and want to get an introcution about the painting "The Great Wave".
RESPONSE:
The Great Wave off Kanagawa is a woodblock print by Japanese ukiyo-e artist Hokusai, created in late 1831 during the Edo period of Japanese history. The print depicts three boats moving through a storm-tossed sea, with a large wave forming a spiral in the centre and Mount Fuji visible in the background.

INPUT:
Now I am looking at the painting "The Greate Wave". Is there any other painting of the same style in this museum.
Reason: The user is during the tour, and want to get related paintings about the painting "The Great Wave" belonging to the same painting style.
RESPONSE:
If you're interested in exploring more artworks with a similar style to "The Great Wave", I recommend taking a look at "Li River in a Splashed-Ink Landscape". This painting also falls under the "Ink Painting" style, which shares some artistic elements with Ukiyo-e and can offer you a captivating experience.

INPUT:
Are there any paintings related to Chinese culture?
RESPONSE:
Yes, there are several paintings related to Chinese culture in the museum. Here are some of them:
1. "Section of Goddess of Luo River" by Zhang Zeduan
   - Author: Zhang Zeduan
   - Country: China
   - Style: Chinese Song Dynasty
2. "Travelers among Mountains and Streams" by Fan Kuan
   - Author: Fan Kuan
   - Country: China
   - Style: Chinese Northern Song
3. "A Man and His Horse in the Wind" by Fu Baoshi
   - Author: Fu Baoshi
   - Country: China
   - Style: Modern Chinese Ink Painting
   - Position: [17.75, 2.57, 17.32]
4. "Forest Grotto at Juqu" by Dong Yuan
   - Author: Dong Yuan
   - Country: China
   - Style: Chinese Southern School
5. "Shen Zhou self portrait at age 80" by Shen Zhou
   - Author: Shen Zhou
   - Country: China
   - Style: Ming Dynasty Painting
These paintings showcase different styles and periods of Chinese art.
"""