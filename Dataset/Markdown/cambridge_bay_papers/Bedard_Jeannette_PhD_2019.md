# Source: http://hdl.handle.net/1828/11357 
# ID: RoleOfVariableConditions

The Role of Variable Oceanographic and Environmental Conditions on Acoustic
Tracking Eﬀectiveness
by
Jeannette Bedard
B.Sc., Royal Roads Military College, 1994
M.Sc., University of Victoria, 2011
A Dissertation Submitted in Partial Fulﬁllment of the
Requirements for the Degree of
DOCTOR OF PHILOSOPHY
in the School of Earth and Ocean Sciences
c⃝Jeannette Bedard, 2019
University of Victoria
All rights reserved. This dissertation may not be reproduced in whole or in part, by
photocopying or other means, without the permission of the author.

ii
Supervisory Committee
The Role of Variable Oceanographic and Environmental Conditions on Acoustic
Tracking Eﬀectiveness
by
Jeannette Bedard
B.Sc., Royal Roads Military College, 1994
M.Sc., University of Victoria, 2011
Supervisory Committee
Dr. Svein Vagle, Co-supervisor
(School of Earth and Ocean Sciences)
Dr. Stan Dosso, Co-Supervisor
(School of Earth and Ocean Sciences)
Dr. Richard Dewey, Departmental Member
(School of Earth and Ocean Sciences)
Dr. David Atkinson, Outside Member
(Department of Geography)

iii
ABSTRACT
Examining ﬁsh behaviour through acoustic tracking is a technique being employed
more and more. Typically, research using this method focuses on detections without
fully considering the inﬂuence of both the physical and acoustic environment. Here we
link the aquatic environment of Cumberland Sound with factors inﬂuencing the detec-
tion eﬀectiveness of ﬁsh tracking equipment and found multi-path signal interference
to be a major issue while seasonal variabilty had little impact. Cumberland Sound
is a remote Arctic embayment, where three species of deep-water ﬁsh are currently
tracked, that can be considered as two separate layers. Above the 300 m deep sill,
the cold Baﬃn Island Current follows a geostrophic pattern, bending into the sound
along the north shore, circulating before leaving along the south shore. The warm
deep water is replenished from the recirculated arm of the West Greenland Current
occasionally ﬂowing over the sill and down to a stable depth. This inﬂux of water
prevents deep water hypoxia, allowing the deep-dwelling ﬁsh populations in the sound
to thrive. To complement the work done in Cumberland Sound, a year-long study of
the underwater soundscape of another Arctic coastal site, Cambridge Bay, Nunavut,
was conducted over 2015. Unlike other Arctic locations considered to date, this site
was louder when covered in ice with the loudest times occurring in April. Sounds
of anthropogenic origin were found to dominate the soundscape with ten times more
snowmobile traﬃc on ice than open water boat traﬃc.

iv
Contents
Supervisory Committee
ii
Abstract
iii
Table of Contents
iv
List of Tables
vii
List of Figures
viii
Acknowledgements
xv
1
Introduction
1
1.1
Study Sites
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
2
1.2
Outline of Thesis . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
4
2
Outside inﬂuences on the water column of Cumberland Sound,
Baﬃn Island
6
2.1
Introduction . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
7
2.2
Data and Methods
. . . . . . . . . . . . . . . . . . . . . . . . . . . .
10
2.2.1
Study Site . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
10
2.2.2
Temperature, Salinity and Depth Proﬁles . . . . . . . . . . . .
13
2.2.3
Moorings
. . . . . . . . . . . . . . . . . . . . . . . . . . . . .
14
2.2.4
Nutrients
. . . . . . . . . . . . . . . . . . . . . . . . . . . . .
15
2.3
Results . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
16
2.3.1
Water above Cumberland Sound’s sill . . . . . . . . . . . . . .
17
2.3.2
Water below Cumberland Sound’s sill . . . . . . . . . . . . . .
24
2.4
Discussion . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
31
2.5
Conclusion . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
34

v
3
On the Variability in Detection Ranges of Passive Acoustic Tags
35
3.1
Introduction . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
35
3.2
Methods . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
39
3.2.1
Characteristics of transmitter tags and receiver arrays . . . . .
39
3.2.2
Tag receiver deployments in Cumberland Sound . . . . . . . .
40
3.2.3
Acoustic Ray-Tracing Model . . . . . . . . . . . . . . . . . . .
44
3.3
Results . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
45
3.3.1
Shallow Range Test . . . . . . . . . . . . . . . . . . . . . . . .
48
3.3.2
Mid-Depth Range Test . . . . . . . . . . . . . . . . . . . . . .
49
3.3.3
Deep Range Test . . . . . . . . . . . . . . . . . . . . . . . . .
49
3.4
Discussion . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
50
3.5
Conclusion . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
56
4
Underwater Soundscape of Cambridge Bay
58
4.1
Introduction . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
58
4.2
Methods . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
61
4.2.1
Location . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
61
4.2.2
Environmental Data
. . . . . . . . . . . . . . . . . . . . . . .
62
4.2.3
Acoustic Recordings
. . . . . . . . . . . . . . . . . . . . . . .
63
4.3
Results . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
64
4.3.1
Environmental Conditions . . . . . . . . . . . . . . . . . . . .
64
4.3.2
Underwater Acoustic Environment
. . . . . . . . . . . . . . .
66
4.4
Discussion . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
69
4.4.1
Biological Sounds . . . . . . . . . . . . . . . . . . . . . . . . .
69
4.4.2
Physical Process Sounds . . . . . . . . . . . . . . . . . . . . .
70
4.4.3
Anthropogenic Sound . . . . . . . . . . . . . . . . . . . . . . .
76
4.4.4
Relative contributions of diﬀerent sources . . . . . . . . . . . .
79
4.4.5
Comparison to other Arctic soundscapes . . . . . . . . . . . .
80
4.5
Conclusion . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
81
5
Conclusion
83
5.1
Oceanography of Cumberland Sound . . . . . . . . . . . . . . . . . .
83
5.2
Detection Range Variability of Passive Acoustic Tags . . . . . . . . .
84
5.3
Cambridge Bay Soundscape . . . . . . . . . . . . . . . . . . . . . . .
85
5.4
Future Directions . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
86

vi
Bibliography
87

vii
List of Tables
Table 2.1 For each year, CTD Cruise dates, number of casts and instruments used.
.
13
Table 2.2 Mooring deployment dates, locations, instruments and depths for the 2010-
2011 bottom thermistors, which were each on their own mooring, and the
2011-2012 North and South Moorings.
Note: RBR XR-420 CT+ on the
North Mooring at 32 m failed 3 Feb 2012
. . . . . . . . . . . . . . . .
15
Table 3.1 Tag types used in this study. Frequency and power output are taken from
the manufacture’s website (http://vemco.com).
. . . . . . . . . . . . .
39
Table 3.2 Receiver and tag depths for each range test. Mooring layouts can be found
in Figure 4.1.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . .
42
Table 4.1 Ocean Sonics icListen HF Hydrophone deployments and locations in Cam-
bridge Bay covering 2015. . . . . . . . . . . . . . . . . . . . . . . . .
63
Table 4.2 Expected and received data by month, where each ﬁle is ﬁve minutes in
duration. The percentage of available data is included in the ﬁnal column. .
64
Table 4.3 Monthly precipitation for 2015 from http://climate.weather.gc.ca. Note, no
data was available for January 2015. . . . . . . . . . . . . . . . . . . .
70
Table 4.4 Environmental parameters and the number of cracks over three 5-minute
samples. For all sample times, water temperature remained relatively stable
between −1.5 and −1.6 ◦C. . . . . . . . . . . . . . . . . . . . . . . .
74

viii
List of Figures
Figure 1.1
Relative locations of study sites. Red box is Cumberland Sound and blue
box is Cambridge Bay.
. . . . . . . . . . . . . . . . . . . . . . .
3
Figure 2.1
A diagram showing the origin of water in Cumberland Sound. The Baf-
ﬁn Island Current (BIC) in blue for the mid-depth layer and the West
Greenland Current (WGC) in orange for the deep layer based on Curry et
al.(2014), arrows entering Cumberland Sound are proposed in this paper.
Rough 200, 500, 1000 and 2000 m isobaths are included. Inset plot shows
temperature-salinity characteristics for both the BIC (blue) and WGC
(red) from data collected in the fall of 2011. . . . . . . . . . . . . . .
9
Figure 2.2
(top) Cumberland Sound bathymetry from the International Bathymet-
ric Chart of the Arctic Ocean (IBCAO) and locations where data were
collected. (bottom) An along-sound depth proﬁle shown as a black line
on the top plot from inland at the sound’s head on the left to the mouth
opening into Davis Strait on the right on the same IBCAO grid. Black
lines with yellow circles mark the mooring locations. Dark blue line marks
where bottom thermistors were deployed and blue line is the location of
the cross-mouth proﬁles. . . . . . . . . . . . . . . . . . . . . . . .
11
Figure 2.3
Ice and meteorology over 2011-2012 at the two mooring sites. The North
Mooring is in grey and the South Mooring is in black. All meteorological
data from NCEP reanalysis. The horizontal axis grid is by month. (a)
Percent ice cover from the Canadian Ice Service weekly ice charts. (b)
Daily average air temperature at 2 m. (c) Daily average wind speeds at 10
m. (d) Along-sound wind speed. (e and f) Wind roses for each mooring
site showing that most winds blow along sound rather than across it. . .
12

ix
Figure 2.4
(a) Temperature-salinity diagram where red is 2011, blue is 2012 and green
is 2013.
Darker markers are average proﬁles and light blue line is the
freezing point of water. Grey lines are potential density. Water masses
from Davis Strait (Curry et al., 2014) are marked as ellipses, dark grey
is Arctic Water (AW), light purple is Transitional Water (TrW), yellow is
West Greenland Irmiger Water (WGIW). (b) to (d) are average potential
temperature, salinity and density proﬁles from 2011-2013. Water masses
from (a) are marked along the right side of (d). . . . . . . . . . . . .
16
Figure 2.5
(a) Potential temperature from 2011 where dots along the top indicate
CTD cast locations. Light grey isopycnals are 1027.2, 1027.3 and 1027.4 kg
m−3, black isopycnal is 1027.5 kg m−3. South mooring is marked and the
black dot is the CT instrument depth. (b) Temperature-salinity diagram
with colour coded 2011 CTD casts (see inset map). Average Cumberland
Sound 2011 CTD proﬁle in red and 2012 CTD proﬁle in blue. Depths are
indicated with markers.
. . . . . . . . . . . . . . . . . . . . . . .
18
Figure 2.6
Nitrate-phosphate relationship for Cumberland Sound from stations in
Figure 2.2 in dark grey where shapes denoted depth to 400 m. Light grey
dots from Davis Strait north of Cumberland Sound taken from Jones et al.
(2003). Known relationships between these nutrients are included for the
Atlantic (solid black line) and Paciﬁc (solid grey line) (Jones et al., 2003).
19
Figure 2.7
Geostrophic velocities at the mouth of Cumberland Sound calculated from
cross-mouth CTD transects. The north shore is on the left so the reader
looks out of the sound towards Davis Strait. Positive is ﬂow into the sound
and negative is ﬂow out. Grey lines are isopycnals starting at 1025.75 kg
m−3 increasing by 0.25 kg m−3 to 1027.25 kg m−3.
. . . . . . . . . .
20
Figure 2.8
2011 CTD data interpolated into horizontal layers with diﬀerent ranges
used on each panel to highlight features at that depth. Locations of CTD
casts used for each depth are indicated by black dots and the CTD casts
too shallow to include are indicated by white dots. Displayed are 20x20
km boxes around each used CTD cast.
. . . . . . . . . . . . . . . .
21

x
Figure 2.9
Time-series plots from the North Mooring where blue shaded area indicates
time of 90% ice cover. Orange highlighted area indicates a wind mixing
event. (a) NCEP reanalysis daily averaged air temperature at 2 m with a
horizontal line at the freezing point of 32 g kg−1 salinity water. (b) NCEP
reanalysis daily average winds at 10 m rotated along the sound. (c) tidal
height from the Webtide model. (d) mooring salinity at 32 m, raw data
is in grey, red line has a 30 hour ﬁlter applied and black line has a 30
day ﬁlter applied. (e) mooring potential temperature time series. A 30
hour ﬁlter has been applied to all the data. Grey lines indicate instrument
depths. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
23
Figure 2.10 (a) Dissolved oxygen from the bottom of the two moorings, north mooring
is grey and south mooring is black. North mooring sensor was at 272 m
and the south mooring’s was at 475 m. The darker line is a 30 day low
pass ﬁlter applied to this data, while the lighter weight line is a 30 hour
ﬁlter. (b) Dissolved Oxygen from CTD casts taken in 2011 in grey and
from 2013 in black.
. . . . . . . . . . . . . . . . . . . . . . . . .
25
Figure 2.11 Contour plot of dissolved oxygen along Cumberland Sound and out into
the Labrador Sea. Light grey isopycnals are 1027.2, 1027.3 and 1027.4 kg
m−3, black isopycnal is 1027.5 kg −3 South mooring at ∼230 km is marked
and the black dot is the CT instrument depth. Dots along the top indicate
cast location following the same scheme as Figure 2.5. . . . . . . . . .
26
Figure 2.12 Time-series plots from the south mooring location. (a) NCEP reanalysis
daily averaged air temperature at 2 m with a horizontal line at the freezing
point of 32 g kg−1 water. (b) NCEP reanalysis daily average winds at 10
m rotated along the sound.
(c) tidal height from Webtide model.
(d)
mooring salinity at 75 m (red) and 275 m (grey), lighter lines are hourly
data, mid-tone lines have a 30 hr ﬁlter applied and darkest lines a 30 day
ﬁlter. (e) mooring potential temperature time series. A 30 hour low pass
ﬁlter has been applied to all data. Grey lines indicate instrument depths
and the black line the depth of the sill. . . . . . . . . . . . . . . . .
28

xi
Figure 2.13 Temperature and salinity data from the south mooring at 275 and 475 m
with tides from Webtide. (a) salinity at 275 m. (b) potential tempera-
ture from both 275 m and the bottom (475 m). (c) tidal height in grey
is indicated on left axis and density in black on right.
Light grey line
marks the deep water threshold of 1027.4 kg m−3. Highlighted period in
September to October 2011 represents a time of deep-water renewal while
the highlighted period in June 2012, no deep-water renewal occurs. . . .
29
Figure 2.14 Schematic diagram illustrating some of the physical processes that oc-
cur through the year in Cumberland Sound. External inﬂuence includes:
geostrophic incursion of the BIC dominating the characteristics of the
above sill layer and deep water pulses of mixed BIC and WGC water re-
plenishing the deep water. Observed internal processes include: estuarine
ﬂow and occasional wind mixing. A requirement for mid-depth processes,
such as internal tides, remains to mix the displaced water and allow it to
exit the sound. . . . . . . . . . . . . . . . . . . . . . . . . . . . .
31
Figure 3.1
(a) Location of the acoustic receivers and transmitters in Cumberland
Sound. Red square is the Deep Range Test, green square is the Mid-depth
Range Test and the blue square is the Shallow Range Test. (b) Sound
speed proﬁles for Cumberland Sound, blue for 2011 and red for 2012. (c-
e) Layout of the three range tests where red dots indicate receiver moorings
and the blue dots the transmitter moorings. The depth and distance scales
are the same in all panels to allow comparison.
. . . . . . . . . . . .
41
Figure 3.2
(a) Percent ice cover from the Canadian Ice Service weekly charts. (b) The
ratio of the total number of pulses to the total number of synchronizations
per day at each site; under ideal conditions this should be greater than
8. (c) Wind speed and air temperature from NCEP reanalysis at all three
sites.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
43
Figure 3.3
Daily detection probability, D, for all three range tests including all tag
types in orange where the colour intensity indicates the number of days
at that D value: more days are darker orange and fewer days are lighter
where the aim was to highlight days with lower D. Blue line is the mean.
45
Figure 3.4
The mean curves of all received tag signals as a function of range for
each test location (blue lines from Figure 3). Coloured bands indicate one
standard deviation from the mean. . . . . . . . . . . . . . . . . . .
46

xii
Figure 3.5
Monthly average range test results for all sites and tags. Thick grey hori-
zontal line is the D=0.5 detection cut oﬀand the thin, light grey vertical
lines are the tag mooring distances from the receiver.
. . . . . . . . .
47
Figure 3.6
Detection probability at two ranges for the Deep V13 test. Blue line is
for 211 m and the orange one for 324 m. Dates range between August to
December 2011. . . . . . . . . . . . . . . . . . . . . . . . . . . .
50
Figure 3.7
Received level as a function of range for the Shallow Range Test where
the horizontal line at 106 dB is the observed detection threshold. Colours
denote tag type: Blue for V9, orange for V13, and green for V16.
. . .
51
Figure 3.8
Model results showing possible paths between the tag and receiver for the
four geometries used in this study. (a) Top left is the Shallow Range Test.
(b) Top right is the Deep Range Test. (c) Bottom left is the Mid-depth
Range Test up the 10% slope and (d) bottom right is the same depth but
down the 10% slope.
. . . . . . . . . . . . . . . . . . . . . . . .
52
Figure 3.9
Time delays between the direct path and secondary paths shown in Figure
3.8 for all three range tests taken from Figure 3.7. Black lines indicate the
end of the blanking period; the ﬁrst line is 260 ms, the second 520 ms, the
third, 780 ms and the fourth, 1020 ms.
. . . . . . . . . . . . . . . .
54
Figure 4.1
Map of Cambridge Bay where the community of the same name is high-
lighted in purple and the location of the underwater platform with a red
dot. Bathymetry is taken from Gade et al. (1974). Inset map is of northern
Canada with the location of Cambridge Bay denoted with a red square. .
61
Figure 4.2
Environmental conditions in Cambridge Bay over 2015. The shore-based
weather station recorded global radiation (a) and air temperature (b).
Water temperature (c), practical salinity (d) and sound speed (e) are from
the CTD on the underwater platform. Fifteen minute averaging was per-
formed on all data and the ice free period from the SWIP is highlighted
in yellow. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
65
Figure 4.3
(a) Ice thickness in metres from the SWIP ice proﬁler.
(b) Year long
spectrogram created from the acoustic recordings with one hour averaging
and 500 Hz frequency bins; colour bar denotes sound power intensity in
dB re 1 µPa2 Hz−1. (c) Wind speed from the weather station. . . . . .
66

xiii
Figure 4.4
(a) Power spectral density in three frequency bands for 15-hour averages
(thin lines) presented along with the monthly average of the band (thicker
line). (b) Power spectral density with the 5th percentile of the spectral
probability density of the quietest month (July 2015) removed over the
full range of frequencies. . . . . . . . . . . . . . . . . . . . . . . .
67
Figure 4.5
Monthly percentiles calculated from the spectral probability densities.
September is greyed out as there were only 8 hours of data recorded that
month.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
68
Figure 4.6
A ﬁve minute spectrogram from 15 August, 2015. The two intermittent
rain events are highlighted with red boxes. The events peak between ∼14
and 15 kHz and reach at most 80 dB re 1 µPa2 Hz−1. . . . . . . . . .
71
Figure 4.7
Winds presented by month in a wind rose format. The rings denote in-
creasing percentage of the time the wind blew in that direction. From the
inside going outward, the rings are 5, 10, 15, 20%. Colours denote wind
speed in m s−1. . . . . . . . . . . . . . . . . . . . . . . . . . . .
72
Figure 4.8
Daily average wind contribution to ambient noise calculated at 3 kHz based
on averaged wind speed for the ice free period in blue. Red is the daily
average in the 3 kHz frequency band.
. . . . . . . . . . . . . . . .
74
Figure 4.9
Ice cracking from 1 April 2015. Top panel is the waveform where the ice
cracking manifests as spikes. The bottom panel is a ﬁve minute spectro-
gram where the PSD of the short ice cracking events reached a maximum
of 87 dB re 1 µPa2 Hz−1 at 5 kHz. . . . . . . . . . . . . . . . . . .
75
Figure 4.10 A typical underwater noise signature of a small boat as observed on 15
July 2015. Top panel shows the raw pressure signal of the passing boat.
The bottom panel is a ﬁve minute spectrogram where the boat sounds
reached a maximum of 145 dB re 1 µPa2 Hz−1. . . . . . . . . . . . .
76
Figure 4.11 Underwater noise signatures from two snowmobiles observed on 1 January
2015. Top panel shows the raw pressure signal in arbitrary units. The
bottom panel shows a ﬁve minute spectrogram where the snowmobile PSD
reached 152 dB re µPa2 Hz−1. . . . . . . . . . . . . . . . . . . . .
77
Figure 4.12 Top is the ice thickness on Cambridge Bay. Bottom are bars for the average
number of vehicle passages (snowmobile or boat, depending on ice cover)
counted over seven days of each month. Black lines are the error bars.
Monthly frequency bands from 4.4a are included layered on top of the
bottom panel with scale on the right side. . . . . . . . . . . . . . . .
78

xiv
Figure 4.13 Relative contribution by frequency of the major contributors to Cambridge
Bay’s soundscape. Both the snowmobiles and boat contributions are taken
from the closest point of approach of three vehicles averaged together. All
others are an average over ﬁve minutes. . . . . . . . . . . . . . . . .
80

xv
ACKNOWLEDGEMENTS
Thank you to my supervisors Svein Vagle and Stan Dosso, for providing guidance
and feedback throughout this project.
My thanks goes out to all who those have helped me complete this dissertation
with whom this project may not have been possible. In particular, I would like to
thank: David Atkinson, Iva Peklova, Kevin Hedges, Nigel Hussey, Aaron Fisk, Nick
Hall-Patch, Mike Dempsey, Sarah Zimmerman, Jenifer Jackson, Bill Williams, Rob
Cooke, Caitlin O’Neil, Emma Murowinski, Marlene Jeﬀeries and Richard Dewey.
I would like to thank the Captain and crew of the R/V Nuliajuk, Ocean Networks
Canada as well as the Ocean Tracking Network project.
Thanks also to my husband Gavin, for putting up with me working in the oﬃce
for hours on end, and for providing guidance and a sounding board when required. I
would also like to thank my daughter Anna for her patience and understanding whilst
doing my work at home. And to my friends Alana, Christine and Amy for forcing me
to occasionally do something fun.

Chapter 1
Introduction
Arctic marine ecosystems experience extremes in light and dark, heat and cold, ice
cover and open water in addition to the tidal forces shared by more southern loca-
tions. These rhythms combine to create habitats where the link between biological
processes and the physical environment are strong (Dayton et al., 1994). Even subtle
hydrographic changes to that environment can profoundly impact the animals that
reside there (Carmack and Wassmann, 2006). In addition to these physical forces,
the underwater acoustic environment, or soundscape, also impacts how ecosystems
function (Staaterman et al., 2013). For Arctic sites, times of ice cover versus open
water can dramatically alter the local soundscape (Kinda et al., 2013).
Because direct observations of aquatic animals are diﬃcult to obtain, especially at
sites experiencing ice cover, implanting passive acoustic tags into these animals is a
method gaining in popularity. This technique is providing new information about how
animals live in their aquatic environments (Hussey et al., 2015; Lennox et al., 2017),
which is especially important in polar ecosystems where climate change is occurring
more rapidly and the animals that live there are more vulnerable due to their slow
growth and low fecundity (Thomas et al., 2008).
By tagging these animals and recording detections, more information can be ob-
tained beyond simple presence and absence. Multiple detections of the same ani-
mal can provide data on movement allowing inferences to be made about animal
behaviours such as habitat use and predator/prey dynamics (Kessel et al., 2013).
However, understanding the limitations of this technique puts the resulting data into
context and prevents incorrect conclusions (Payne et al., 2010; Kessel et al., 2013).
One major limitation is the detection range of the receivers which is variable and
impacted by both the equipment and the local environment.

2
The objective of this thesis is to gain a better understanding of the link between
Arctic coastal oceanography and soundscapes and the impact on the range of passive
detection of acoustically-tagged ﬁsh. To achieve this, three main questions are posed
and addressed:
1. What are the oceanographic processes that deﬁne the underwater environment
in an Arctic coastal embayment?
2. What sounds dominate the underwater soundscape of the site?
3. How do the oceanography and soundscape impact acoustic tag function in the
local environment?
By answering these questions, biological behaviour recorded by the acoustic tags
can be put into context with the physical and acoustic environment. In this study in
situ data collection, water sampling and acoustic ray-tracing modelling are used to
obtain results.
1.1
Study Sites
The original intent of this research project was to consider all three questions at a
single site, Cumberland Sound, a large embayment on the east coast of Baﬃn Island
(Figure 1.1).
The work was started as part of a cross-discipline team within the
Ocean Tracking Network (OTN) (Cooke et al., 2011). Biologists addressing questions
around habitat use for three species of ﬁsh were included in addition to oceanogra-
phers. However, at the end of the ﬁrst year (summer 2012) all moored equipment was
removed at the request of the local Inuit community, before any underwater acoustic
recordings were made. Unfortunately, only a single year of mooring data was col-
lected (2011-2012). Surface-based measurements were not part of the ban, allowing
collection to continue resulting in three years of summer data (2011-2013).
As a result of being unable to collect acoustic data in Cumberland Sound, the
soundscape component of this work was performed in Cambridge Bay (Figure 1.1),
another Arctic coastal site. Cambridge Bay has an underwater platform collecting
data as part of the Ocean Networks Canada (ONC) array. This platform hosts a
variety of oceanographic instruments measuring aspects of the local environment as
well as a continuously-recording hydrophone. Data from 2015 were chosen, because

3
Figure 1.1:
Relative locations of study sites.
Red box is Cumberland Sound and blue box is
Cambridge Bay.
the acoustic data were nearly complete (the exception being September 2015) and
that year coincided with an acoustic range test of the same acoustic tags used in
Cumberland Sound. The original intent of moving to this site was to expand the
evaluation of range tests to a very shallow (∼9 m water depth) location. However,
the range test data set proved to be unusable as it was heavily contaminated by a
nearby ship running a 50 kHz sonar from May until September. This range test was
abandoned and a shift was made to consider the soundscape instead.
Unfortunately, Cumberland Sound and Cambridge Bay are fundamentally diﬀer-
ent sites with only seasonal ice cover and their position within the Canadian Arctic
Archipelago (CAA) in common. Cumberland Sound is a large embayment heavily
inﬂuenced by outside water. The sound is ∼80 km wide by ∼250 km long with a 300
m deep sill and maximum depths of over 1400 m. In comparison Cambridge Bay is
more complex in shape. Near the study site it is ∼4 km wide by ∼3 km long, with
maximum depths of only 86 m.
There is an ongoing anthropogenic presence at both of these sites involving ﬁshing
activity and vehicle use.
In Cumberland Sound, Greenland halibut (Reinhardtius

4
hippoglossodies) is a ﬁsh species of commercial interest with population dynamics
that are only beginning to be understood (Peklova et al., 2012). While in Cambridge
Bay, vehicle noise at times dominates the aquatic environment which, in general, has
been shown to have a negative eﬀect on the fauna (Williams et al., 2015).
1.2
Outline of Thesis
This thesis is based on three papers written to address each of the questions posed
above. Details of the methodologies used are presented in each paper.
1. The ﬁrst paper (Chapter 2) discusses the outside inﬂuences on the water col-
umn of Cumberland Sound. This is the ﬁrst oceanographic work in this location
where even the bathymetry is not fully known. The water column in the sound
is divided into two layers: the water above the 300 m deep sill, and the water
below. Two diﬀerent mechanisms are presented that bring in diﬀerent water
masses. The ﬁrst is geostrophic ﬂow cycling through the upper layer and the
second is seasonal, intermittent deep water replenishment that prevents the bot-
tom waters from becoming hypoxic. Local processes that contribute to mixing
within the sound are also presented. This paper has been published as Bedard
et al. (2015).
2. The second paper (Chapter 3) examines the variability in detection ranges of
passive acoustic tags in an Arctic embayment. Three year-long range tests were
performed with acoustic ﬁsh tags in Cumberland Sound, with tags programmed
to transmit at known intervals deployed at a variety of ranges from receivers.
Results from these range tests are linked with factors inﬂuencing the detection
eﬀectiveness using a simple ray tracing model. Multi-path interference is found
to be a major issue impacting detections while seasonal variability is not an
issue at this site. This paper will be submitted to Animal Biotelemetry.
3. The ﬁnal paper (Chapter 4) presents results from a year-long study of the
soundscape in Cambridge Bay.
Unlike other Arctic locations considered to
date, this site is louder when covered in ice with the loudest times occurring in
April. Sounds of anthropogenic origin are found to dominate the soundscape
with roughly ten times more snowmobile traﬃc on ice than open-water boat

5
traﬃc. Precipitation, wind and ice noise are the other major contributors and
non-human biological sources are not found to be signiﬁcant.
The following chapters were written as stand-alone papers with their own intro-
duction, methods, results, discussion and conclusion sections.

6
Chapter 2
Outside inﬂuences on the water
column of Cumberland Sound,
Baﬃn Island
Cumberland Sound, host to a commercially viable ﬁsh population in the deepest
depths, is a large embayment on the southeast coast of Baﬃn Island that opens
to Davis Strait. Conductivity, temperature and depth proﬁles were collected dur-
ing three summer ﬁeld seasons (2011-2013) and two moorings were deployed during
2011-2012. Within the sound, salinity increases with increasing depth while water
temperature cools reaching a minimum of −1.49 ◦C at roughly 100 m. Below 100
m, the water becomes both warmer and saltier. Temperature-salinity curves for each
year followed a similar pattern, but the entire water column in Cumberland Sound
cooled from 2011 to 2012, then warmed through the summer of 2013. Even though
the sound’s maximum depth is over a kilometre deeper than its sill, water in the
entire sound is well oxygenated. A comparison of water masses within the sound and
in Davis Strait shows that, above the sill, the sound is ﬂooded with cold Baﬃn Island
Current water following an intermittent geostrophic ﬂow pattern entering the sound
along the north coast and leaving along the south.
Below the sill, replenishment
is infrequent and includes water from both the Baﬃn Island Current and the West
Greenland Current. Deep water replenishment occurred more frequently on spring
tides, especially in the fall of 2011. Although the sound’s circulation is controlled by
outside currents, internal water modifying processes occur such as estuarine ﬂow and
wind-driven mixing.

7
2.1
Introduction
The tight link between physical and biological processes found in Arctic aquatic
ecosystems (Dayton et al., 1994) creates an environment where even subtle hydro-
graphic changes can profoundly impact local biological activity (Carmack and Wass-
mann, 2006). Located on the cusp of the Arctic Circle, Cumberland Sound’s benthic
ecosystem is especially vulnerable to change. Currents containing water from both
the Paciﬁc and Atlantic Oceans cross the sound’s mouth (Jones et al., 2003), while its
shallow sill is poised to cut oﬀmost of the water column. However, a kilometre below
the depth of the sill, a permanent population of Greenland Halibut (Reinhardtius
hippoglossoides) reside (Peklova et al., 2012). These ﬁsh are harvested in the only
community-run commercial Greenland Halibut ﬁshery in Nunavut, providing needed
economic support to the small Inuit community of Pangnirtung. In addition, this ﬁsh-
ery is being used as a model to create similar ﬁsheries in other northern communities.
As we will show, Cumberland Sound is periodically renewed by intrusions of dense,
mixed shelf water supplying oxygen to support the Greenland Halibut and their as-
sociated ecosystem. The sound’s renewal dynamics depend on the temperature and
salinity of the currents passing across Cumberland Sound’s mouth. As these currents
change with our changing climate (Steiner et al., 2013), the sound’s ecosystem will
also change.
Previous observations of physical water properties within Cumberland Sound are
sparse: a naturalist from the Smithsonian spent a winter there in 1877-78 (Kumlien,
1879) observing the ﬂora and fauna while taking meteorological measurements, and
in 1952, Dunbar (1958) sampled temperature and salinity at three stations across the
mouth of the sound. Dunbar found a temperature minimum around 100 m and no
evidence of geostrophic ﬂow in and out of the sound. Since 1952, no further sam-
pling has been reported. Even though no oxygen measurements have been previously
reported in Cumberland Sound, based on the existence of a bottom dwelling popula-
tion of Greenland Halibut in the sound (Peklova et al., 2012), we can assume that the
deepest regions are not hypoxic. However, oxygen levels may be low, as Greenland
Halibut have been found in regions with 18–25% oxygen saturation and can survive
down to 15% in laboratory studies (Dupont-Prinet et al., 2013).
Cumberland Sound opens to southwestern Davis Strait (Figure 2.1) where roughly
equal quantities of Paciﬁc- and Atlantic-origin water transit heading south (Jones
et al. 2003; Lique et al. 2010). Several properties distinguish these water masses.

8
Paciﬁc origin water is colder and fresher than the warmer, saltier Atlantic origin
water (Jones et al., 2003). Additionally, Paciﬁc origin water contains less nitrate
than Atlantic water creating diﬀerent relationships between nitrate and phosphate,
a ratio which is conserved and can be used to identify a water mass origins (Jones
et al., 1998). Due to higher sea levels in the Paciﬁc, water ﬂows from the Paciﬁc
across the Arctic Ocean to the Atlantic (Carmack, 2007). Once in the Arctic Ocean,
Paciﬁc water ﬂows east along the north coast of North America (Rudels 2012; Hu
and Myers 2013), before passing through the Canadian Arctic Archipelago’s (CAA)
maze of channels (Prinsenberg and Bennett 1989; Jones et al. 2003; McLaughlin et al.
2004; Michel et al. 2006; Rudels 2012). This ﬂow exits into Baﬃn Bay, a large body of
water between northern Baﬃn Island, southern Ellesmere Island and the west coast
of Greenland, joining the cyclonic ﬂow pattern within the bay (Tang et al. 2004; Cuny
et al. 2005).
Once in Baﬃn Bay, Paciﬁc and Arctic Ocean origin water mix, becoming ‘Arctic
Water’ (AW) (Cuny et al. 2005; Curry et al. 2014). AW (θ ≤2 ◦C and S ≤33.7 g
kg−1) remains in the surface layer (< 300 m) incorporating winter cooling remnants
in a temperature minimum around 100 m (Tang et al., 2004). On the Greenland
side of Baﬃn Bay, denser Atlantic-origin water moves away from the coast, sliding
beneath the colder, but lighter AW (Bacle et al., 2002) becoming Transitional Water
(TrW) (θ > 2 ◦C and S > 33.7 g kg−1) (Cuny et al. 2005; Curry et al. 2014). With
an interface around 300 m, these two layers ﬂow south, hugging Baﬃn Island, as the
Baﬃn Island Current (BIC). Some of the southward ﬂowing BIC water recirculates
north of Davis Strait (e.g. Myers and Ribergaard 2013; Gladish et al. 2015). The
BIC ultimately crosses Cumberland Sound’s mouth (Tang et al., 2004; Curry et al.,
2014) (Figure 2.1).
Flowing north along the Greenland coast through Davis Strait into Baﬃn Bay, is
the West Greenland Current (WGC). This current carries two distinct water masses
ﬂowing side-by-side (Curry et al., 2014). Arctic origin ‘West Greenland Shelf Water’
(WGSW) (θ < 7 ◦C and S < 34.1 g kg−1) ﬂows along the Greenland coast. Adjacent
to the WGSW along the West Greenland slope, ﬂows West Greenland Irmiger Water
(WGIW) (θ > 2 ◦C and S > 34.1 g kg−1) of Atlantic origin. At the southern edge of
Davis Strait the WGC splits, with one part continuing north through the strait and
the other part turning westward (Cuny et al. 2002; Fratantoni and Pickart 2007; Myers
et al. 2009) (Figure 2.1). The westward arm crosses Davis Strait before circulating
southward adjacent to the BIC roughly 100 km away from Cumberland Sound’s mouth

9
Figure 2.1:
A diagram showing the origin of water in Cumberland Sound.
The Baﬃn Island
Current (BIC) in blue for the mid-depth layer and the West Greenland Current (WGC) in orange
for the deep layer based on Curry et al.(2014), arrows entering Cumberland Sound are proposed in
this paper. Rough 200, 500, 1000 and 2000 m isobaths are included. Inset plot shows temperature-
salinity characteristics for both the BIC (blue) and WGC (red) from data collected in the fall of
2011.
(Cuny et al. 2002; Fratantoni and Pickart 2007; Myers et al. 2009).
The BIC and WGC’s velocity, temperature and salinity vary on an annual ba-
sis (Curry et al., 2014).
The BIC follows a seasonal cycle with peak currents in
October-November and ﬂow reversals in the winter (Curry et al., 2014). In the Arctic
Water (AW) layer of the BIC, salinity reaches a maximum in May and a minimum in
January while temperature reaches a maximum in August and a minimum in April
(Curry et al., 2014). AW density ranges from roughly 1025 to 1027.2 kg m−3. The
West Greenland Irminger Water (WGIW) water mass in the WGC also follows a
seasonal pattern with maximum currents occurring between October and December
and a density range of 1027.3 to 1027.8 kg m−3. Salinity peaks twice, once between

10
October and January and the second time between March and May. The WGIW
reaches a temperature maximum in late fall and a minimum over the summer. At
the front between the AW in the BIC and WGIW in the WGC, the WGC is always
denser. Although signiﬁcant inter-annual variability has been observed in both the
BIC and WGC, no clear inter-annual trends have been identiﬁed (Curry et al., 2014).
Both currents ﬂow past the mouth of Cumberland Sound, and have the potential to
inﬂuence water within the sound.
The objectives of this paper are: (a) to identify the origins of the water in Cum-
berland Sound and (b) to describe the physical water properties in the sound. In
Section 2 we describe the physical setting, data collection, meteorological and ice
conditions of the sound. In Section 3, the sound is split into above and below sill
layers to discuss the origins and physical processes inﬂuencing each layer. In Section
4, possible consequences of long term changes are discussed.
2.2
Data and Methods
2.2.1
Study Site
Cumberland Sound is a coastal body of water on the south coast of Baﬃn Island
roughly 80 km wide and 250 km long following a northwest-southeast axis (Figure
2.2).
At the southeast end, Cumberland Sound opens into Davis Strait.
At the
mouth, the sill is part of the Baﬃn Island Shelf and reaches ∼300 m in depth, and
half of the sounds total volume is below the sill depth. The steepest bathymetry
occurs along the north coast where the sounds depth drops from the coast to ∼150
m in 17 km (slope of 0.07). Within the sound there are two deep, muddy-bottomed
pockets, one reaching ∼800 m and the other ∼1400 m with a 300 m deep ridge
separating them. Although there are no major rivers, several seasonal small rivers
along with glacier runoﬀempty into the sound. Small islands litter the periphery of
the sound and several fjords open out into it. Like Frobisher Bay and Hudson Strait
to the south, Cumberland Sound has very strong tides. The tides have a 6 m range
and are dominated by semi-diurnal oscillations (M2) modulated by the spring-neap
cycle (Webtide Model, Hannah et al. 2008). At the sound’s mouth barotropic tidal
velocities reach 0.18 m s−1, and exceed 0.2 m s−1 in the narrow channels between the
islands and the coast (Webtide Model, Hannah et al. 2008).
Cumberland Sound is far enough north to experience complete winter ice cover,

11
Figure 2.2: (top) Cumberland Sound bathymetry from the International Bathymetric Chart of the
Arctic Ocean (IBCAO) and locations where data were collected. (bottom) An along-sound depth
proﬁle shown as a black line on the top plot from inland at the sound’s head on the left to the mouth
opening into Davis Strait on the right on the same IBCAO grid. Black lines with yellow circles mark
the mooring locations. Dark blue line marks where bottom thermistors were deployed and blue line
is the location of the cross-mouth proﬁles.
however, within the sound, regions of open water typically remain all winter (Figure
2.3a). On average, January open water area is ∼34 km2, less than 1% of the total
area of the sound (Barber and Massom, 2007). Ice cover above the two mooring
sites during 2011-2012 was very similar (Figure 2.3a). Weekly ice cover proportions
from the Canadian Ice Service archives (http://www.ec.gc.ca/glaces-ice/) indicate
that the sound had 50% ice cover over the entire sound by 28 November 2011 and
90% by December 5th. By 19 December 2011, fast ice began forming along the shores

12
Figure 2.3: Ice and meteorology over 2011-2012 at the two mooring sites. The North Mooring
is in grey and the South Mooring is in black. All meteorological data from NCEP reanalysis. The
horizontal axis grid is by month. (a) Percent ice cover from the Canadian Ice Service weekly ice
charts. (b) Daily average air temperature at 2 m. (c) Daily average wind speeds at 10 m. (d)
Along-sound wind speed. (e and f) Wind roses for each mooring site showing that most winds blow
along sound rather than across it.
of the sound and by early January 2012, the sound contained primarily fast ice. In
May 2012 an open area formed midway along the south shore and cycled open and
closed until the sound became ice free. In the same month, at the north end of the
sound, areas of reduced ice cover appeared then closed by early June. Ice began
retreating in July leaving the sound mostly ice free by late August.
Daily mean composites of wind and air temperature were obtained from NOAA’s
National Centre for Atmospheric Prediction (NCEP) North American Regional Re-
analysis Composites (Kalnay et al., 1996). Air temperature was taken at 2 m and wind

13
Year
Date Range
Number
Instrument
of Casts
2011
23 July-26 August
44
Seabird SBE 19 CTD
with a SBE-43 DO sensor
2012
17 August-20 September
31
Seabird SBE 19 CTD
2013
13 August-3 September
9
RBR XR-620 CTD
with JFE Alex Co.Ltd Rinko DO sensor
Table 2.1: For each year, CTD Cruise dates, number of casts and instruments used.
velocity at 10 m (Figure 2.3). Air temperature in Davis Strait was generally higher,
resulting in warmer air in the sound during periods with southerly winds. Northerly
winds oﬀof the land mass of Baﬃn Island were typically colder. In Cumberland
Sound, winds predominately blew along the axis of the sound (Figure 2.3e and f) and
the wind directions were similar at both mooring sites. The u and v components of
the wind were rotated 160◦into an along-sound and across-sound coordinate system
where positive values indicate wind blowing into the sound (Figure 2.3d). Since, the
along-sound winds were signiﬁcantly stronger than cross-sound ones, the cross-sound
winds are ignored. The wind blew predominately out of the sound (Northerly winds)
from September 2011 until mid April 2012. From mid April until August 2012, the
wind blew on average into the sound (Southerly winds). Although, the winds followed
this pattern over long time scales, on a daily basis there was signiﬁcant variability in
direction. The strongest winds were found in the fall, switching direction every few
days.
2.2.2
Temperature, Salinity and Depth Proﬁles
Ship-based conductivity, temperature and depth (CTD) proﬁles were collected as
part of the Ocean Tracking Network (OTN) project each summer from 2011 to 2013.
CTD cast locations and instruments used for each year can be found in Figure 2.2 and
sampling dates and instruments are listed in Table 2.2. A diﬀerent CTD instrument
was used each year and each instrument’s sensors were calibrated at the beginning
of each ﬁeld season. The CTD instrument also included a dissolved oxygen sensor
in 2011 and 2013, in both years these sensors were calibrated before and after use.
In 2012, two cross-mouth transects were performed a month apart (17 Aug 2012 and
20 Sept 2012) along the same line Dunbar (1958) sampled in 1952. The same seven

14
stations were sampled on each transect with an average spacing between stations of
9 km. Both transects took 6 hours to complete and were done on opposite phases
of the tide. Additional CTD data were collected for Labrador Sea directly out from
Cumberland Sound (transect shown in Figure 2.5b) by the University of Washington
using a a Seabird 911+ instrument.
Conductivity, temperature and depth data from 2011 and 2012 were processed
the same way. First, CTD corrections were made using the Seabird software, then
upcasts and downcast were separated out. The upcasts and downcasts were compared
and found to be almost the same. In 2011, the downcast data were used. In 2012 the
upper 50 m of CTD data was bad because the pump was not on. These data were
removed and replaced with data from the upcasts. In 2013, CTD casts were taken at
ﬁsh survey locations (Figure 2.2) with an instrument limited to depths above 740 m.
A preset conductivity threshold was programmed to start and stop the instrument
which resulted in frequent missed sampling in the upper layers. Up and down casts
were compared and found to be very similar. To compensate for missed upper layer
samples, up-casts were used. Each years data were averaged into 1-m bins from which
potential temperature, salinity and density were calculated. The mean of each 1-m
depth bin was taken to create an average cast for each year.
2.2.3
Moorings
In the summer of 2010, OTN acoustic ﬁsh tagging began and lines of acoustic receiver
moorings were deployed in Cumberland Sound at the northern end of the sound
(Figure 2.2).
Eleven RBR TR-1050 thermistors set to sample every minute were
attached on the bottom of these moorings. These moorings ranged in depth between
178 and 385 m and were recovered during the 2011 ﬁeld season.
Two dedicated oceanographic moorings were deployed from 2011-2012 (Figure
2.2), mooring locations and composition are listed in Table 2.
One mooring was
deployed at the north, inland end of the sound in close proximity to the line of
thermistors from 2010-2011. This site, referred to as the ‘north mooring’, was situated
between two deep pockets that are known ﬁsh habitats. Water depth was 272 m and
the ﬂoat extended to 32 m below the surface. At the mouth of the sound, the south
mooring was deployed to sample water entering the sound from Davis Strait. This
mooring sat at a depth of 475 m with the ﬂoat 75 m below the surface. Both moorings
were recovered in the summer of 2012. The conductivity temperature (CT) sensor

15
Mooring
Date Range
Location
Instrument
Depth [m]
Bottom
17 Aug 2010-
65.96◦N 66.38◦W
RBR TR-1050 thermistor
178
Thermistors
10 July 2011
65.97◦N 66.38◦W
RBR TR-1050 thermistor
188
65.97◦N 66.35◦W
RBR TR-1050 thermistor
223
65.98◦N 66.41◦W
RBR TR-1050 thermistor
229
65.94◦N 66.29◦W
RBR TR-1050 thermistor
232
65.95◦N 66.32◦W
RBR TR-1050 thermistor
237
65.95◦N 66.26◦W
RBR TR-1050 thermistor
271
65.96◦N 66.32◦W
RBR TR-1050 thermistor
275
65.93◦N 66.26◦W
RBR TR-1050 thermistor
291
65.92◦N 66.24◦W
RBR TR-1050 thermistor
374
65.95◦N 66.35◦W
RBR TR-1050 thermistor
385
North
24 Aug 2011-
65.99◦N 66.53◦W
RBR XR-420 CT+
32
Mooring
1 Sept 2012
RBR TR-1050 thermistor
57
RBR TR-1050 thermistor
82
RBR TR-1050 thermistor
107
RBR TR-1050 thermistor
157
RBR TR-1050 thermistor
182
RBR TR-1050 thermistor
207
RBR TR-1050 thermistor
232
RBR DO-1050
272
South
1 Sept 2011-
64.77◦N 63.99◦W
RBR XR-420 CT+
75
Mooring
2 Sept 2012
RBR TR-1050 thermistor
57
RBR TR-1050 thermistor
100
RBR TR-1050 thermistor
125
RBR TR-1050 thermistor
150
RBR TR-1050 thermistor
175
RBR TR-1050 thermistor
225
RBR XR-420 CT+
275
RBR TR-1050 thermistor
325
RBR TR-1050 thermistor
375
RBR DO-1050
475
Table 2.2: Mooring deployment dates, locations, instruments and depths for the 2010-2011 bottom
thermistors, which were each on their own mooring, and the 2011-2012 North and South Moorings.
Note: RBR XR-420 CT+ on the North Mooring at 32 m failed 3 Feb 2012
on the north mooring failed 3 February 2012 for unknown reasons. All instruments
were from RBR Ltd. and programmed to sample every minute. Accuracy for the
temperature sensors was ±0.002 ◦C, for the conductivity sensors ±0.003 mS cm−1
and for the dissolved oxygen sensor ±2%.
2.2.4
Nutrients
Water samples were collected at four sites along the length of the sound in August
2012 (Figure 2.2). At each site, duplicate samples were taken at the surface, 100
m, 200 m and 400 m using a horizontal acrylic water sampler. The samples were
frozen for transport to the Institute of Ocean Sciences in Sidney, B.C. Each sample
was analyzed for nitrate+nitrite, phosphate and silicate using a Three Channel Tech-
nicon Autoanalyzer, only the nitrate+nitrite and phosphate are used here. For the
nitrate+nitrite the duplicate diﬀerence mean was 0.90 µm l−1 and standard deviation
was 0.68 µm l−1. For the phosphate the duplicate diﬀerence mean was 0.14 µm l−1
and standard deviation was 0.24 µm l−1. Duplicate results were averaged together.

16
2.3
Results
Figure 2.4: (a) Temperature-salinity diagram where red is 2011, blue is 2012 and green is 2013.
Darker markers are average proﬁles and light blue line is the freezing point of water. Grey lines
are potential density. Water masses from Davis Strait (Curry et al., 2014) are marked as ellipses,
dark grey is Arctic Water (AW), light purple is Transitional Water (TrW), yellow is West Greenland
Irmiger Water (WGIW). (b) to (d) are average potential temperature, salinity and density proﬁles
from 2011-2013. Water masses from (a) are marked along the right side of (d).
Between 2011 and 2013, Cumberland Sound’s water column changed, however
each year there were shared features (Figure 2.4). The water reached a near freez-
ing temperature minimum around 100 m, a typical characteristic of Arctic waters
(Melling, 2002). Below the temperature minimum, water became warmer and saltier
with depth, creating an upturn in the temperature-salinity curve (Figure 2.4a) im-
plying diﬀerent origin water intruded into the sound (Dunbar, 1958). Bottom density
did not exceed 1027.5 kg m−3 (Figure 2.4a and d), which will be important when
considering bottom water renewal. From 2011 to 2013 the water column cooled and
freshened at all depths. At 800 m, the result was water cooled by ∼1 ◦C and freshened

17
by ∼0.2 g kg−1 (Figure 2.4b and c).
Even though Cumberland Sound’s deepest region is more than a kilometre below
the sill, the sound is not isolated at any depth from the surrounding waters travers-
ing Davis Strait. On the temperature-salinity diagram (Figure 2.4a) ovals indicating
water masses from Davis Strait are included from Curry et al. (2014). Within Cum-
berland Sound, water with properties similar to AW, TrW and WGIW are found
(Figure 2.4a). Above the sill (∼300 m) the sound is directly linked to outside water,
while below the sill, there is a pool of water only occasionally replenished. Since these
two layers are subject to diﬀerent processes, the sill, which is really the Baﬃn Island
Shelf, is used to separate the sound into upper and lower layers.
2.3.1
Water above Cumberland Sound’s sill
Above the sill, Cumberland Sound is ﬂooded with water from the BIC. This is demon-
strated by the potential temperature contours shown in Figure 2.5a. Cold BIC water
ﬂows southward along the ∼100 km wide shelf that forms the sound’s sill (Figure
2.5a). Within the sound, below the ∼25 m deep warmer layer at the surface, water
of similar temperature as the BIC extends across the entire above-sill layer (Figure
2.5a). Temperature-salinity proﬁles show that although water in the sound is slightly
warmer and fresher than water of the same depth in the BIC (Figure 2.5b), in gen-
eral, the above sill water in Cumberland Sound matches the AW layer of the BIC
suggesting an ongoing interaction with the BIC. Additional evidence of AW water
within the sound is found in the nutrient ratios (Figure 2.6).
Arctic Water is composed of a combination of Paciﬁc and Atlantic origin waters.
Although biological processes modify nutrient concentrations (Cooper et al., 1999),
the nutrient levels between Paciﬁc and Atlantic waters are diﬀerent enough to al-
low the identiﬁcation of water mass origins across the Arctic to the Labrador Sea
(Jones et al., 1998). Here, nitrate and phosphate ratios are used to infer the origins
of the above sill layer in Cumberland Sound (Figure 2.6). Paciﬁc and Atlantic ratios
are included as lines in Figure 2.6. Values that fall on these lines suggest a water
mass composed of that origin water, while values in between the lines imply mixing
occurred. For comparison, nutrient relationships from Davis Strait, north of Cumber-
land Sound are included (Jones et al., 2003). In the BIC, the fraction of Paciﬁc origin
water ranged from 30 to 60%, decreasing with depth (Jones et al., 2003). Above the
sill, Cumberland Sound contains 40% Paciﬁc water, similar to Davis Strait (Jones

18
Figure 2.5: (a) Potential temperature from 2011 where dots along the top indicate CTD cast
locations. Light grey isopycnals are 1027.2, 1027.3 and 1027.4 kg m−3, black isopycnal is 1027.5 kg
m−3. South mooring is marked and the black dot is the CT instrument depth. (b) Temperature-
salinity diagram with colour coded 2011 CTD casts (see inset map). Average Cumberland Sound
2011 CTD proﬁle in red and 2012 CTD proﬁle in blue. Depths are indicated with markers.
et al., 2003), providing further evidence that the upper layer of the sound is ﬁlled
with AW from the BIC.
Geostrophic Flow
In this section geostrophic ﬂow is considered as the mechanism bringing BIC water
into the above-sill layer of Cumberland Sound. The relatively wide width of the sound
compared to the Rossby radius suggests a component of the south-moving BIC water
enters the sound. The ﬁrst-mode baroclinic Rossby radius (Lr) is:
Lr = NHf −1
(2.1)

19
Figure 2.6:
Nitrate-phosphate relationship for Cumberland Sound from stations in Figure 2.2
in dark grey where shapes denoted depth to 400 m. Light grey dots from Davis Strait north of
Cumberland Sound taken from Jones et al. (2003). Known relationships between these nutrients are
included for the Atlantic (solid black line) and Paciﬁc (solid grey line) (Jones et al., 2003).
where N is the buoyancy frequency (N = 5.4 x 10−3 s−1), H is the depth (H =
300 m, approximate depth of the sill) and f is the Coriolis (f = 1.3 x 10−4 s−1). Here,
only the lowest mode Rossby radius was considered. For the mouth of Cumberland
Sound, the Rossby radius was 12 km, or approximately 1/6 of the width of the sound.
Using density proﬁles from the two 2012 cross-mouth transects, ﬂow patterns
in and out of Cumberland Sound were deduced (Figure 2.7).
For each transect,
geostrophic ﬂow was calculated assuming no net transport through the section which
required small bottom ﬂows of 0.007 m s−1 for the ﬁrst transect and 0.002 m s−1 for
the second. Transect 1 followed the expected pattern where a component of the BIC
entered the sound along the north shore and exited along the south shore. Mid-sound,
currents were very weak. By transect 2, a month later, the ﬂow pattern through the
mouth of the sound changed dramatically, now water ﬂowed into the sound along the
south shore, and ﬂow out was concentrated in the top 100 m centre-sound. Below
100 m, little water movement occurred.
Further evidence of this variability was
found in 1952 when Dunbar (1958) looked for a geostropic ﬂow pattern but did not
observe one at the time of his cross-mouth transect. However, the three stations

20
Figure 2.7: Geostrophic velocities at the mouth of Cumberland Sound calculated from cross-mouth
CTD transects. The north shore is on the left so the reader looks out of the sound towards Davis
Strait. Positive is ﬂow into the sound and negative is ﬂow out. Grey lines are isopycnals starting at
1025.75 kg m−3 increasing by 0.25 kg m−3 to 1027.25 kg m−3.
Dunbar used were roughly 25 km apart; greater than the Rossby Radius, making it
possible a geostrophic ﬂow pattern was missed. The observed changing ﬂow patterns
demonstrates variability in how water enters and leaves the sound, but does not
quantify the frequency of any speciﬁc pattern. However, from the two transects, we
conclude for at least some of the time, the BIC enters the sound.
Using the geostrophic velocities, a rough residence time for the above-sill layer
can be computed. Here we assume that the transport through the sound’s mouth
is constant, that the water circulates around the entire sound and that there is no
interaction below the sill. The International Bathymetric Chart of the Arctic Ocean
(IBCAO) was used to calculate the volume of the above-sill layer and the volume
of water entering the sound was taken from transect 1 (Figure 2.7). Residence time
can be deﬁned by system capacity volume divided by the volume transport, for the
above-sill layer of Cumberland Sound this works out to roughly 30 days if the transect
1 ﬂow pattern was constant. Since, we have already determined there is variability in
the ﬂow pattern through the sound’s mouth, we can only say that the residence time
for this layer is on the order of months.

21
Internal Structure of Cumberland Sound
Figure 2.8: 2011 CTD data interpolated into horizontal layers with diﬀerent ranges used on each
panel to highlight features at that depth. Locations of CTD casts used for each depth are indicated
by black dots and the CTD casts too shallow to include are indicated by white dots. Displayed are
20x20 km boxes around each used CTD cast.
In addition to the outside inﬂuence from water masses in Davis Strait, lateral
mapping of the sound’s water properties indicates estuarine circulation also occurs.
The 2011 data will be discussed since CTD casts that year had the widest horizontal
coverage of the sound. Near the surface, water was fresher towards the sound’s head,
forming a plume with a sharp gradient adjacent to Pangnirtung Fjord (Figure 2.8a).
For most of the fresh water plume, water was warmer than that outside the plume
except the northernmost, inland CTD cast taken at the mouth of a seasonal river
where water was more than a degree colder (Figure 2.8b).
As this surface water

22
ﬂowed away from the source it is possibly warmed by contact with the atmosphere.
The plume’s presence suggests fresh water from glacier runoﬀand small seasonal
rivers plays a role in driving water movement outwards from the head of the sound.
At 150 m, below the sound’s temperature minimum, lower salinity water was
observed along the north coast compared to centre sound (Figure 2.8c). This lower
salinity water was also colder towards the mouth of the sound (Figure 2.8d). A pocket
of higher salinity, warmer water was found under the surface freshwater plume at the
sound’s head suggesting estuarine ﬂow may occur in this region drawing up deeper
warm water to replace that pushed out by the surface freshwater plume. At 400 m,
water was nearly uniform in salinity, with water along the north coast only ∼0.05 g
kg−1 saltier (Figure 2.8e). This more saline water was also warmer (Figure 2.8f). The
gradients of diﬀerent water along the north coast at both 150 m and 400 m suggest
an inﬂux of outside water followed the isobaths and modiﬁed within the sound. The
less saline, colder water along the coast at 150 m is likely AW and the slightly more
saline and warmer water found in the same location at 400 m could be TrW.
The spatial plots in Figure 2.8 show the two moorings were situated in diﬀer-
ent oceanographic regimes. The north mooring was directly beneath the fresh water
plume in the path of possible estuarine ﬂow. At the mouth of the sound, the south
mooring was in the path of outside water entering the sound. To determine how
related observations at the two moorings were, coherence was calculated between
thermistors located at similar depths at each mooring (not shown) (Bendat and Pier-
sol, 2010). The coherence function evaluates how similar two diﬀerent functions are.
Temperature data was interpolated into hourly increments then coherence was calcu-
lated over 21, 42, 85 and 120 days to ﬁnd common frequencies. Signiﬁcant coherence
(>0.6) only occurred at tidal frequencies, conﬁrming the moorings experienced dif-
ferent regimes.
North Mooring Temperature and Salinity Structure
Over 2010-2011 near bottom water on-average warmed over the year (not shown),
then in 2011-2012 water cooled, with the warmest temperatures observed in Septem-
ber 2011 and coldest in May 2012 (2011-2012 shown in Figure 2.9e). The opposite
changes over these two years suggests that an annual cycle of warming and cooling fol-
lowing the seasons did not occur in Cumberland Sound. The most notable diﬀerence
between the two years was the average wind direction. Over the fall of 2010, winds

23
Figure 2.9: Time-series plots from the North Mooring where blue shaded area indicates time of
90% ice cover. Orange highlighted area indicates a wind mixing event. (a) NCEP reanalysis daily
averaged air temperature at 2 m with a horizontal line at the freezing point of 32 g kg−1 salinity
water. (b) NCEP reanalysis daily average winds at 10 m rotated along the sound. (c) tidal height
from the Webtide model. (d) mooring salinity at 32 m, raw data is in grey, red line has a 30 hour
ﬁlter applied and black line has a 30 day ﬁlter applied. (e) mooring potential temperature time
series. A 30 hour ﬁlter has been applied to all the data. Grey lines indicate instrument depths.
on average blew into the sound, bringing in warmer air from Davis Strait, keeping air
temperatures above the freezing point of water for salinity 32 g kg−1 (1.75 ◦C) until
the end of November 2010. Ice did not reach 50% cover until mid-February 2011.
During the fall of 2011, winds on average blew out of the sound bringing cold air
from the mountainous terrain on Baﬃn Island over the sound. This year, ice formed
much earlier. By December 2011, this region of the sound experienced 90% ice cover
(Figure 2.3a).
Over 2011-2012 temperature decreased at the north mooring, most rapidly in
November 2011 and March 2012 (Figure 2.9e). Between these times the water column

24
only slightly cooled. Since the only salinity measurements were taken at 32 m on an
instrument that failed in February, the focus will be on Fall 2011. From September
2011 to February 2012, salinity increased at 32 m by ∼0.5 g kg−1 mostly over two
separate time-frames (Figure 2.9d). The ﬁrst salinity increase occurred between 4
September and 10 October 2011 when water temperatures were above 1.5 ◦C and the
warmest in the water column (Figure 2.9e). This suggests the salinity increase during
this time period was not the result of ice formation, therefore another process must
have been responsible.
Between 13 and 16 November 2011, a mixing event occurred that rapidly cooled
the top ∼182 m by ∼1 ◦C (highlighted in orange on Figure 2.9). This event was
likely wind driven and is the only observation of a surface process inﬂuencing deeper
layers. On 13 November 2011, wind reversed from blowing out of the sound and
began blowing into the sound bringing in warmer air from Davis Strait (Figure 2.9b).
As the wind speed increased, air temperature increased above the freezing point, but
still colder than the surface water, prompting cooling by ∼0.5 ◦C. In the top 182
m, the water appeared homogeneous in temperature suggesting the breakdown of
stratiﬁcation. By 15 November 2011, well mixed, cold water reached a depth of 232
m, while the upper layers continued to cool. One day later, this cooling reached the
bottom and surface layers cooled by ∼1 ◦C, renewing the whole water column. On
18 November 2011, wind reversed again, now blowing out of the sound, and the air
temperature rapidly dropped well below the freezing point (Figure 2.9a).
Salinity increased more rapidly starting around 20 November 2011 and levelled oﬀ
by 22 January 2012 (Figure 2.9d). During this time, temperatures at the same depth
approached the freezing point. At the surface, ice cover reached 90% by 3 December
2011 (Figure 2.3a), suggesting the second salinity increase may have been inﬂuenced
by brine rejection from ice formation.
2.3.2
Water below Cumberland Sound’s sill
Cumberland Sound’s sill cuts oﬀthe lower layer from direct interaction with water
masses in Davis Strait. Down to the deepest pockets in potentially isolated regions of
the sound lives a population of Greenland Halibut (Peklova et al., 2012). Therefore
deep water renewal must occur often enough to prevent hypoxia. As hypoxia threshold
deﬁnitions vary (Hofmann et al., 2011), here a mid-value of 20% dissolved oxygen
(DO) saturation will be used which is within the range that Greenland Halibut have

25
Figure 2.10: (a) Dissolved oxygen from the bottom of the two moorings, north mooring is grey
and south mooring is black. North mooring sensor was at 272 m and the south mooring’s was at
475 m. The darker line is a 30 day low pass ﬁlter applied to this data, while the lighter weight line
is a 30 hour ﬁlter. (b) Dissolved Oxygen from CTD casts taken in 2011 in grey and from 2013 in
black.
been found elsewhere (Dupont-Prinet et al., 2013). For Cumberland Sound’s bottom
temperatures, 20% DO saturation corresponds to an oxygen concentration of roughly
3 mg l−1. The deepest (1127 m) DO measurement, taken in 2011, was well above this
hypoxia threshold (Figure 2.10b). Dissolved oxygen measurements were taken again
in 2013 to a depth of ∼800 m. Although there was more variability this year, values
were similar to those observed in 2011.
At both mooring sites bottom DO concentrations decreased throughout 2011-
2012 (Figure 2.10a), likely due to a combination of respiration and decomposition
as biological activity continued beneath the ice in limited daylight. Some inﬂux of
oxygenated water likely occurred in February and March 2012, possibly through an
intrusion of outside water. Alternately, convection or deep mixing might play a role
at the north mooring location. Assuming the observed decrease typical of DO use
in Cumberland Sound, below sill renewal must occur to replenish the oxygen. To
roughly estimate how long oxygen in the deep pockets of Cumberland Sound could
last without renewal, the 0.3 mg l−1 DO decrease over 2011-2012 at the bottom of
the south mooring (475 m) (Figure 2.10a) was assumed to mirror deep water oxygen

26
changes. By continuing to decrease DO without renewal, bottom water would become
hypoxic in four years. Therefore, to maintain Cumberland Sound’s ecosystem, deep
water must be replenished at intervals shorter than four years.
Below the sill, the sound collects incursions of water from the strait. This region
has characteristics similar to TrW water from the lower layer of the BIC, however
this water is not warm or dense enough to be the sole source of the deepest waters.
On the other side of the BIC, warmer, dense water is found in the WGIW component
of the WGC. WGIW water reaches 5 ◦C, much warmer than Cumberland Sound’s
deep water maximum measured temperature of 2.9 ◦C. Thus, at the same salinity,
the deepest temperatures in the sound were warmer than the BIC (region shown in
light green, Figure 2.5b) but not as warm as the WGC (region shown in grey, Figure
2.5b), implying the deep water is a mixture of these two water types.
Figure 2.11: Contour plot of dissolved oxygen along Cumberland Sound and out into the Labrador
Sea. Light grey isopycnals are 1027.2, 1027.3 and 1027.4 kg m−3, black isopycnal is 1027.5 kg −3
South mooring at ∼230 km is marked and the black dot is the CT instrument depth. Dots along
the top indicate cast location following the same scheme as Figure 2.5.
The front between the cold BIC and warm recirculated arm of the WGC possibly
creates the water destined to become Cumberland Sound deep water. In September
2011, this front was located roughly 100 km across the sill of the sound (Figure
2.5a) and contained a sharp temperature gradient over the entire water column while

27
density increased from the BIC into the WGC. On the WGC side of the front, water
denser than 1027.5 kg m−3 was observed at a depth of 300 m (isopycnal denoted in
black on Figure 2.5b) roughly 100 km away from the sound. This water was denser
than the water observed within Cumberland Sound.
Therefore, the sound’s deep
water likely originates from the sound side of the 1027.5 kg m−3 isopycnal where
BIC and WGC waters interact. Across the sill into the Labrador Sea, DO levels
are generally greater that those found below the sill within the sound (Figure 2.11).
There is a pocket of lower DO found near the bottom across the sill at the interface
between the BIC and WGC (below dark green dots on Figure 2.11) providing further
evidence the below sill water in the sound originates from this region.
South Mooring Temperature and Salinity Temporal Structure
Like the north mooring, the water column at the south mooring decreased in tempera-
ture over 2011-2012 (Figure 2.12e). Concurrently, the water column freshened, which
was more pronounced at 75 m (decrease by 0.39 g kg−1) than at 275 m (decrease by
0.18 g kg−1) (Figure 2.12d). These changes are consistent with changes in average
CTD casts observed between summer 2011 and 2012 (Figure 2.4). On a seasonal
timescale, the whole water column cooled in the late fall, lasting from late November
2011 until January 2012, however the bottom waters still remained warmer than those
above, suggesting an outside inﬂuence or moving front between water masses rather
than a wind mixing event like that observed at the north mooring (Figure 2.9). This
fall-to-winter cooling also corresponds to the lowest temperatures in the TrW layer
of the BIC ﬂowing across the mouth of the sound (Curry et al., 2014) and is likely a
combination of both outside inﬂuence and local mixing. After this time, water below
sill depth warmed while the upper layer remained cool, perhaps in response to the
lower temperatures in the AW layer of the BIC that occur at this time (Curry et al.,
2014). Another cooling event occurs in mid-July 2012 which was contrary to the
general timing of maximum temperatures in the BIC observed June-August (Curry
et al., 2014).
The most striking feature of this time series are the ﬂuctuations found in both
the temperature and salinity (Figure 2.12d and e). Spectral analysis indicates that
the most energetic periods correspond to tidal frequencies and are dominated by the
spring-neap and M2 tides. To check if these ﬂuctuations were the result of mooring
movement, the mooring was modelled using the Mooring Design and Dynamics Mat-

28
Figure 2.12:
Time-series plots from the south mooring location.
(a) NCEP reanalysis daily
averaged air temperature at 2 m with a horizontal line at the freezing point of 32 g kg−1 water.
(b) NCEP reanalysis daily average winds at 10 m rotated along the sound. (c) tidal height from
Webtide model. (d) mooring salinity at 75 m (red) and 275 m (grey), lighter lines are hourly data,
mid-tone lines have a 30 hr ﬁlter applied and darkest lines a 30 day ﬁlter. (e) mooring potential
temperature time series. A 30 hour low pass ﬁlter has been applied to all data. Grey lines indicate
instrument depths and the black line the depth of the sill.
lab package (Dewey, 1999). For a maximum tidal velocity of 0.3 m s−1 (maximum
modelled tides at this location reached 0.18 m s−1) the top of the mooring experienced
a maximum vertical excursion of 4 m, which is less than 1% of water depth. Further
conﬁrmation that the mooring did not signiﬁcantly move vertically comes from the
bottom temperature sensor located at 1 m oﬀthe bottom (Figure 2.13b) that follows
the same ﬂuctuations as the sensors above. Therefore, it is assumed the observed
ﬂuctuations are not the result of instruments moving through diﬀerent vertical layers
in the water column.

29
Figure 2.13: Temperature and salinity data from the south mooring at 275 and 475 m with tides
from Webtide. (a) salinity at 275 m. (b) potential temperature from both 275 m and the bottom
(475 m). (c) tidal height in grey is indicated on left axis and density in black on right. Light grey line
marks the deep water threshold of 1027.4 kg m−3. Highlighted period in September to October 2011
represents a time of deep-water renewal while the highlighted period in June 2012, no deep-water
renewal occurs.
Deep Water Replenishment
In this section, we will show that deep water replenishment in Cumberland Sound
occurred most often during spring tides in the fall of 2011.
The 2011 and 2012
temperature-salinity curves from the sound were compared to the 2011 data extend-
ing out into the Labrador Sea (Figure 2.5a). No CTD casts were performed in the
Labrador Sea in 2012. The south mooring CT sensor at 275 m, 200 m above the
bottom (shown on Figure 2.5b), was situated to measure external water entering the
sound. Over 2011-2012, the mooring water was of similar salinity, but colder than
the summer CTD casts taken farther into the sound. Salinity at 275 m ﬂuctuated
following the spring-neap tides (Figure 2.13a). Compared to CTD casts from within
the sound (Figure 2.4c), these ﬂuctuations represent a depth range between 400-900

30
m, meaning that at 275 m, just above sill depth, water destined to ﬁll most of the
lower layers regularly passes.
Most of the year, water passing the mooring contained predominately BIC water,
slightly cooler than the September 2011 BIC water denoted in light green in Figure
2.5b. At times, water passing the mooring, warmed and became denser, but never
as warm and dense as the WGC water, suggesting mixed BIC/WGC water entered
the sound. The densest water that passed the mooring matched water from 300 m
in the BIC/WGC front (dark green lines in Figure 2.5b) and the deepest water in
the sound. It is likely that this external water passing the mooring mixed with and
cooled, or displaced, the deep water already in the sound resulting in the cooling
observed between 2011 and 2012 (Figure 2.4b).
Over 2011-2012, density at 275 m decreased (Figure 2.13c). In general, water dense
enough to become bottom water passed the mooring infrequently (bottom water cut
oﬀdensity of 1027.4 kg m−3, Figure 2.13c) in pulses that were also warmer (Figure
2.13b). These warm pulses only reached a maximum of 2.5 ◦C, and never as warm as
the 2.9 ◦C bottom temperatures measured in 2011. Although, these pulses of warm,
dense bottom water occurred into May 2012, they were most frequent in the fall of
2011 and corresponded to spring tides (Figure 2.13c) suggesting stronger currents
associated with these tides play an important role in mixing the front water and
facilitating its entry into the sound. Water was generally less dense on the neap tides
(Figure 2.13c) and water entering the sound was predominately BIC water. Overall,
roughly 75% of the time water passing the mooring was not dense enough to become
bottom water.
At the sound’s mouth, maximum barotropic tidal velocities from the Webtide
Model reached 0.18 m s−1 (Hannah et al., 2008). A rough determination of how far
water moves on each tide, the horizontal tidal excursion (E), was calculated using:
E = UoTπ−1
(2.2)
where Uo is the horizontal velocity and T is the tidal period. Maximum tidal excursion
at the mouth of the sound was 2.6 km, much less than the shelf width (∼300 km),
suggesting tidal ventilation could not have acted alone to ventilate the deep water.
‘Pulses’ containing the densest and warmest water were observed September to
November 2011 on the spring tides. An example spring tide from October 2011 is
highlighted in Figure 2.13. Here water dense enough to be bottom water passes more

31
frequently than during a comparable spring tide from June 2012, also highlighted
(Figure 2.13). Considering the 30 hr ﬁltered density for both time periods, water
dense enough to be bottom water passed the mooring in November 2011 and not
during June 2012. The fall is the time of peak currents for both the BIC and WGC.
At this time, the BIC gets denser due to ice formation and further reduces the density
diﬀerence with the WGC. This seasonal variability, suggests that deep water in the
sound is the result of tidal mixing of the BIC and WGC at times when the variability
in these currents allows the water to ﬂow into the sound.
2.4
Discussion
Figure 2.14: Schematic diagram illustrating some of the physical processes that occur through
the year in Cumberland Sound.
External inﬂuence includes: geostrophic incursion of the BIC
dominating the characteristics of the above sill layer and deep water pulses of mixed BIC and
WGC water replenishing the deep water. Observed internal processes include: estuarine ﬂow and
occasional wind mixing. A requirement for mid-depth processes, such as internal tides, remains to
mix the displaced water and allow it to exit the sound.
Although Cumberland Sound is an isolated sub-Arctic embayment, the currents
crossing the sound’s mouth subject the interior to inﬂuences from water masses from
across the sub-Arctic. A schematic of the identiﬁed physical processes in Cumberland
Sound is shown in Figure 2.14. Above the sill, cold, fresh water from the BIC circulates
through the sound.
Below the sill, lower layers are replenished intermittently by
a warm, saline mix of BIC and WGC water masses. Although, external physical
processes dominated, the observations suggested several internal processes, including

32
estuarine ﬂow and wind mixing, are also present. However, it is postulated that the
observed internal processes are incomplete and other unidentiﬁed physical processes
that reach to the mid-water column are required to explain the observations (Figure
2.14). Pulses of dense water were observed entering the sound, destined to replenish
the deepest regions.
However, evidence of water leaving the deep areas was not
observed. As dense water entering the sound displaces what is already there, a link
must exist between the deepest layers and the layers above. Since most of Cumberland
Sound below the sill is horizontally homogeneous, any deep water displaced upwards
must cool and freshen, implying that mixing occurs in the mid-water column below
the temperature minimum but well above the bottom. The fate of the displaced deep
water has not yet been determined, but it is possible that it rises to just above the
sill and is entrained below the BIC water leaving the sound.
Several internal processes may impact the mid-depth water in the sound, such as
observed wind mixing and estuarine circulation, and not-observed sinking of dense
water from ice formation and internal tides. Infrequent wind mixing events inﬂuenced
water to depths greater than 200 m. Typically these events lasted a few days and were
accompanied by strong winds that switched direction. One event was observed in the
fall of 2011 and two in the fall of 2010. Over the entire sound, wind mixing events
might reach deep enough to facilitate cooling of the displaced deep water. Estuarine
ﬂow is another potential mechanism bringing up deeper water. At the head of the
sound, a fresh water plume was observed pushing out from a seasonal river source.
At ∼150 m, a pocket of warmer water with higher salinity was observed beneath the
fresh water plume above, perhaps drawn up from below. Not observed, but likely
playing a role, is sinking cold, salty water that is rejected when ice forms. Finally,
the mid-depth water is likely impacted by the strong tidal currents in the area. It
is postulated that these currents get converted into strong internal tides within the
sound. It is likely that a combination of all these processes facilitates the removal of
displaced bottom water from the lower layers.
Changes within either the BIC or WGC have the potential to make a signiﬁcant
impact on the aquatic ecosystem of Cumberland Sound. For example, a signiﬁcant
freshening of the BIC from ice melt further north could increase the stratiﬁcation
above the sill and therefore potentially blocking the inﬂux of dense bottom water
into the sound. Within a few years the sound could become hypoxic and unable to
support the current ecosystem. The likelihood of these changes occurring is diﬃcult
to determine. A freshening trend of 0.15 g kg−1 per decade has been observed in

33
Baﬃn Bay/Davis Strait (Steiner et al., 2013). One source of this fresh water is from
melting glaciers in Greenland (Steiner et al., 2013). Between 1992-2010 the rate of
discharge from Greenland’s glaciers accelerated and is expected to keep accelerating
(Rignot et al., 2011). Closer to Cumberland Sound, Hamilton and Wu (2013) predict
the BIC will freshen by 0.4 g kg−1 by 2050.
These predictions would strengthen
stratiﬁcation in the sound and increase the probability that replenishment would be
cut oﬀin the below-sill layer.
Alternately, if the BIC warmed or more WGC water entered the sound, bot-
tom temperatures could rise slightly. Warmer water could change how ﬁsh use the
area. For example, currently Greenland Halibut use Cumberland Sound as a forag-
ing ground where they stay a few years before moving on (Peklova et al., 2012). To
date, no evidence of spawning Greenland Halibut has been found in the sound (Kevin
Hedges, pers. coms.) implying these ﬁsh leave to spawn. A known spawning ground
exists in Davis Strait outside the sounds mouth in water warmer than 3 ◦C (Scott
and Scott, 1988). Only a slight warming of Cumberland Sounds bottom water would
result in water consistently above 3 ◦C, potentially changing the sound from a rich
foraging ground to a spawning ground.
There is no other sub-Arctic site comparable to Cumberland Sound. Near by, only
Frobisher Bay and Hudson Strait share similar widths as Cumberland Sound, are also
in the ﬂow path of the BIC and WGC, and have strong tidal currents, but neither
provide a good comparison. Frobisher Bay, just south of the sound, is at most less
than half the depth of Cumberland Sound and little previous work has been done
here. Monitoring done in Hudson Strait (Straneo and Saucier, 2008) shows that a
geostrophic ﬂow through the strait occurs following the same pattern as Cumberland
Sound; westward along the north shore and eastward along the south shore. However,
Hudson Strait is part of a the larger Hudson Bay system (Straneo and Saucier, 2008)
with much stronger tidal currents (∼1 m s−1 in Hudson Strait vs ∼0.2 m s−1 in
Cumberland Sound) preventing useful comparisons between the two locations.
Cumberland Sound is a complex and unique environment where inﬂuences from
various sources have impacts down to the deepest depths. The water origins and
important physical processes that deﬁne water within the sound have been identiﬁed.
However, the internal processes have not been fully deﬁned, nor has the impacts of
changes to the BIC or WGC on the sound, leaving important questions unanswered
for future study.

34
2.5
Conclusion
Water properties in Cumberland Sound are inﬂuenced by outside water of two dif-
ferent origins, the Baﬃn Island Current and the West Greenland Current. Above
the sill, the cold Baﬃn Island Current follows a geostrophic pattern, bending into
the sound. This water enters along the north shore, circulating the sound and leaves
along the south shore, however at times this pattern is interrupted. The warm deep
water is replenished from the recirculated arm of West Greenland Current occasion-
ally ﬂowing over the sill and down to a stable depth. This process is variable and
occurs most frequently on spring tides.
This inﬂux of water prevents deep water
hypoxia, allowing the deep-dwelling ﬁsh populations in the sound to thrive.

35
Chapter 3
On the Variability in Detection
Ranges of Passive Acoustic Tags
Examining ﬁsh behaviour through passive acoustic tracking is a technique being em-
ployed more and more. Typically, research using this method focuses on detections
without fully considering the inﬂuence of the environment. Here we linked the aquatic
environment of Cumberland Sound with factors inﬂuencing the detection eﬃciency of
ﬁsh tracking equipment and ﬁnd multi-path signal interference to be a major issue.
Cumberland Sound is a remote Arctic embayment where three species of deep-water
ﬁsh are currently tracked. Detection ranges obtained through a series of year-long
acoustic functionality experiments (range tests) are combined with two-dimensional
ray tracing model results to examine the eﬀect of both environmental and equipment
related factors on detection ranges.
3.1
Introduction
How aquatic animals use their environment is an important question, but direct
observations are diﬃcult to obtain resulting in large data gaps. To ﬁll these gaps a
number of telemetric techniques have been developed. One method, passive acoustic
animal tracking, is providing new information about how animals live in aquatic
environments (Hussey et al., 2015; Lennox et al., 2017). In these studies, animals are
surgically implanted with an acoustic transmitter, or ‘tag’, and receivers are deployed
at critical locations where the animals might pass. When the tagged animal moves
within the detection range of a receiver, its identity is recorded along with the time,

36
creating an animal presence snapshot. Multiple detections of tagged animals provide
information on movement and habitat use patterns, and inferences can be made about
animal behaviours, such as migration patterns and predator/prey dynamics (Kessel
et al., 2013). A review of studies from 1986 to 2012 found an exponential growth in
the use of passive acoustic animal tracking (Kessel et al., 2013), causing this technique
to be described as a ‘revolution’ (Hussey et al., 2015).
Passive acoustic telemetry is based on the following assumptions: 1. the experi-
ence of being tagged does not aﬀect the behaviour of the animal being tracked, 2.
equipment performance does not bias the data, and 3. the resulting detection data
are representative of the animal’s behaviour (Singh et al., 2009). Also, it is often
assumed the rate of signal attenuation with distance is stable over space and time,
something that generally is not true (Gjelland and Hedger, 2013).
Unfortunately, past studies typically do not fully consider passive acoustic tagging
limitations.
Kessel et al. (2013) found that researchers considered the variability
of detection eﬀectiveness in 42% of 378 studies evaluated, but only 18% discussed
the inﬂuence of multiple external factors. However, reliable inferences about tagged
animal behaviour can only be made in conjunction with an understanding of the
dynamic nature of detection eﬀectiveness for any particular study area (Payne et al.,
2010; Kessel et al., 2013).
Determining how well an in situ passive acoustic telemetry system is able to de-
tect tagged animals is critical for data interpretation. This includes quantifying the
maximum detection range, that is, how far away from a tag the signal is reliably
received.
With the exception of occasional false detections (Simpfendorfer et al.,
2015), one can be reasonably conﬁdent that a detection means a tagged animal is
present. However, if no detections are recorded, one cannot be certain that no tagged
animals are present because detection eﬀectiveness is typically less than 100% and
can change with environmental variability and equipment set up. Physical and tech-
nical limitations must be considered when interpreting detection data. The main
issues impacting detection eﬀectiveness are the environmental conditions, including
ambient noise, multi-path interference, and the electrical and mechanical nature of
the detection equipment. These issues are interconnected, and all play a role; thus,
quantifying detection eﬀectiveness requires addressing all of these factors.
As a tag’s signal propagates from transmitter to receiver it is inﬂuenced by the
local environment which can result in ﬂuctuating detection eﬀectiveness. First, the
signal spreads geometrically as it propagates, causing the sound intensity to decrease

37
at a rate of 1/r2 (spherical spreading) or 1/r (cylindrical spreading), where r is
the range, depending on whether propagation is bottom/surface bounded. Further,
propagating signals can be absorbed and/or scattered, or even blocked by obstacles
(Medwin and Clay, 1998). In addition to the direct path, a signal may reﬂect oﬀof
boundaries resulting in the same signal following many paths between transmitter and
receiver. These multi-path arrivals can interfere with the time coding of the signal
used by the receiver to identify a speciﬁc tag and cause the receiver to fail to register
a signal reception. Finally, the receiver must pick the signal out from the background
noise, which also depends on the environmental conditions. Environmental factors
change with time, further complicating data interpretation and making predictions
about what factors impact detection eﬀectiveness a diﬃcult, site-speciﬁc problem
(Huveneers et al., 2016; Ottera and Skilbrei, 2016).
The impacts of a changing environment on how tag signals are received are only
beginning to be systematically examined. For example, Heupel et al. (2006) found
that receivers located in adjacent estuarine and freshwater environments had a sig-
niﬁcant detection range diﬀerence, dropping from ∼800 m in the estuary to ∼600 m
in freshwater. In another study, at diﬀerent depths in a fjord, a combination of wave
action and stratiﬁcation resulted in detection ranges varying between 45 and 650 m
(Finstad et al., 2005). Detection eﬀectiveness was also found to vary due to turbu-
lence and entrained bubbles (Thorstad et al., 2000). Clements et al. (2005) found
that detection eﬃciency in shallow water (less than 21 m) was much greater for a
ﬂowing-water system than a static-water system, possibly due to background noise.
This relates to the discussion by Kessel et al. (2015) of how low noise environments
with a hard boundary (e.g. the water-air interface or the bottom) can result in short-
range interference between diﬀerent path signals, resulting in fewer than expected
detections.
The optimum design and implementation of a passive acoustic animal tracking
experiment is site-speciﬁc.
In a recent study, Huveneers et al. (2016) found that
receiver depth and orientation, along with time since deployment (potentially related
to biofouling), had more inﬂuence on detection range than other environmental factors
they considered (wind, precipitation, atmospheric pressure and water temperature).
Detection ranges vary with tag transmitter power, with the detection ranges of lower-
powered transmitters decreasing much faster than those of higher-powered tags (How
and de Lestang, 2012). How a receiver is mounted on the mooring line can block
incoming acoustic signals (Clements et al., 2005) and receiver performance can be

38
negatively impacted by biofouling (Heupel et al., 2008; Ottera and Skilbrei, 2016;
Huveneers et al., 2016).
A way to study these issues is to perform an acoustic functionality experiment,
also known as a detection range test, at the study location. In such a test, a series of
tags programmed to transmit at regular intervals are deployed at known ranges from
a receiver. The purpose is to determine how eﬀectively signals are recorded over time
at diﬀerent ranges to quantify the variability of the system (Singh et al., 2009). In
addition, the impact of multi-path interference can be evaluated. These tests can also
help to determine the optimum number of tags and the optimum receiver spacing,
and can provide insight into possible causes of unexpected results (Singh et al., 2009).
Quantifying how receivers respond to local conditions through a detailed analysis
of range test data can provide conﬁdence in negative results (Udyawer et al., 2013).
However, Kessel et al. (2013) found that although most of 378 acoustic tagging studies
reviewed performed a range test, only 13% published the results. Additionally, the
scope of these range tests varied from a single tag held in place for a few days to
comprehensive testing with a series of tags deployed for the study period of up to a
year (Kessel et al., 2013).
Another tool to examine both the eﬀects of environmental factors and acoustic
tracking geometry is acoustic modelling. Although acoustic models are widely used
in other applications, they are only beginning to be applied to passive acoustic animal
tracking studies. Melnychuk and Walters (2010) used an attenuation model to predict
the number of ﬁsh that passed an array but were not detected. Gjelland and Hedger
(2013) showed that the probability of detecting a tagged animal could be modelled
using attenuation coeﬃcients and general sound propagation theory. Huveneers et al.
(2016) took a statistical approach by estimating the range at which detection proba-
bilities drop below 50% from ﬁtting detection data with a logistic or sigmoidal curve.
How and de Lestang (2012) found a 3-parameter sigmoidal model provided the best
ﬁt. Despite these few reported studies, acoustic modelling as a tool to understand
and interpret acoustic telemetry data is currently underused.
Here we investigate the environmental issues aﬀecting the detection eﬀectiveness
of three range tests deployed in Cumberland Sound, Baﬃn Island, between 2011 and
2012. This work was conducted as part of an Ocean Tracking Network (OTN) study
(Cooke et al., 2011) to monitor a commercially viable ﬁsh population in the sound
(Peklova et al., 2012). The detection eﬀectiveness at various ranges is evaluated and
interpreted using a simple acoustic ray tracing model demonstrating the impact of

39
multi-path interference. Even though the present analysis is focused on data from
Cumberland Sound, the methodology from this work is applicable to passive acoustic
telemetry studies in other locations.
3.2
Methods
3.2.1
Characteristics of transmitter tags and receiver arrays
The acoustic tags and receivers used in this study were manufactured by Vemco Ltd.,
Bedford, Nova Scotia, Canada, and are in wide use (Kessel et al., 2013). Diﬀerent
sizes of tags are available, creating a trade-oﬀbetween tag size, battery life and power
output. The tags chosen for a speciﬁc study depend on the target animal size and
project data requirements; therefore, in any given scenario, multiple tag types may
be used. In this study three types of tag (V9, V13 and V16) with a range of source
levels were used (Table 3.1).
Tag Type
Frequency
Power Output
[kHz]
[dB re 1 µPa2 Hz−1]
V9
69
145-151
V13
69
147-153
V16
69
150-162
Table 3.1: Tag types used in this study. Frequency and power output are taken from the manu-
facture’s website (http://vemco.com).
The basic operation of both animal tracking and range testing tags is the same.
All tags transmit a unique acoustical time coded signal at a ﬁxed frequency. The
diﬀerence between ﬁsh and range test tags is the time intervals between transmissions.
For ﬁsh tags, the signals are transmitted at random intervals to minimize the chance
of two tags transmitting simultaneously within a receiver’s range. In contrast, range
test tags are programmed to transmit at a constant known interval. In this study,
all tags were programmed with a ﬁxed time interval between signal transmissions of
between 1770 and 1830 s. Using a carrier frequency of 69 kHz, tag transmissions
consist of two synchronization pulses followed by a unique identiﬁcation (ID) code
made up of a series of six pulses. Each pulse contains roughly 34 cycles of the carrier
frequency, resulting in pulses ∼5 ms long. The times between the various pulses form
the ID code, with a total of seven digits. Each transmitted ID code is at least 1.9 s

40
in duration.
When the VR2W-69 kHz receiver detects the ﬁrst pulse it turns oﬀ(sleeps) for
260 ms to minimize interference from other signals and other tags. Following this
delay the receiver turns back on and waits for the next pulse in the ID sequence.
Once the second pulse is detected the receiver again sleeps for 260 ms before waiting
for the third pulse.
This process repeats for all eight pulses.
The receiver then
performs a checksum, or error check, to determine if a valid signal was detected. If a
signal is validated, the date, time and ID code of the tag are stored in the receiver.
Incomplete or non-existent ID codes may be detected for several reasons. The same
signal following another path, a diﬀerent signal or even noise of the right frequency
to interfere with the signal can result in an invalid ID code or a false detection. It is
also possible that a tagged animal can move through a region that blocks part of the
signal (Simpfendorfer et al., 2008). Each receiver generates detection data summary
statistics every 24 hours including the total number of pulses, the total number of
synchronization intervals, and the number of checksum errors received per day. The
total number of pulses divided by the total number of synchronizations should be
equal to eight under ideal conditions. Any signiﬁcant departure from eight indicates
transmission collisions, path interference or signiﬁcant ambient noise levels in the
vicinity of the receiver.
3.2.2
Tag receiver deployments in Cumberland Sound
In support of ﬁsh tracking eﬀorts in Cumberland Sound, three year-long range tests
were performed from August 2011 to August 2012 (Figure 4.1).
The aim was to
determine the temporal variability and environmental dependence of detection ranges
over a year in a range of water depths from 100-1000 m. To set the stage for acoustic
propagation at the site, sound speed proﬁles were computed from the average of a
set of summer 2011 and 2012 CTD (conductivity, temperature and depth) proﬁles
(Figure 4.1b). At a depth of approximately 100 m there is a sound speed minimum,
below which the sound speed increases steadily to the bottom creating an upward
refractive acoustic environment. The shapes of the sound speed proﬁles were the
same in the two years; note that both are summer proﬁles and winter conditions
would be diﬀerent. However, the speeds were lower in 2012 due to the colder water
that year. A more detailed oceanographic discussion of the sound can be found in
Bedard et al. (2015).

41
Figure 3.1: (a) Location of the acoustic receivers and transmitters in Cumberland Sound. Red
square is the Deep Range Test, green square is the Mid-depth Range Test and the blue square is
the Shallow Range Test. (b) Sound speed proﬁles for Cumberland Sound, blue for 2011 and red for
2012. (c-e) Layout of the three range tests where red dots indicate receiver moorings and the blue
dots the transmitter moorings. The depth and distance scales are the same in all panels to allow
comparison.
The range test layouts are summarized in Figure 4.1c-e and in Table 3.2. The ﬁrst
range test was conducted at a depth of 1000 m in the southern, deep-water region
of the sound, and is referred to as the Deep Range Test (red square in Figure 4.1a).
This test included both V16 and V13 tags. The second range test was conducted in
∼400 m of water in the northern end of Cumberland Sound and is referred to as the
Mid-depth Range Test (green square in Figure 4.1a). This test was performed in a
location with 10% slope to the sea ﬂoor and used all three tag types. The third test
was conducted in ∼100 m of water inside Pangnirtung Fjord (blue square in Figure
4.1a), using all three tag types, and is referred to as the Shallow Range Test. Three

42
Range
Mooring
∼Instrument
∼Height Above
Tags Used
Test
Type
Depth [m]
Bottom [m]
Shallow
Receiver
100
20
Tag
100
10
V9, V13, V16
Mid-Depth
Receiver
400
13
Tag
400
10
V9, V13, V16
Deep
Receiver
1000
182
Tag
1000
10
V13, V16
Table 3.2: Receiver and tag depths for each range test. Mooring layouts can be found in Figure
4.1.
receiver moorings, each with a downward facing receiver, were deployed in as straight
a line as possible at each site. The ﬁrst receiver was set at a predetermined location,
the second receiver at a horizontal distance of 800 m and the third receiver at 1000
m (Figure 4.1c-e). Between the ﬁrst two receiver moorings, ﬁve shorter moorings
with one of each type of tag used at the site were deployed at regular intervals. This
combination resulted in a total of 15 tag-receiver ranges. Note, for the Mid-depth
Range test the farthest receiver did not record any data and it was assumed to have
failed.
Each mooring consisted of a 100 kg anchor attached by a 2 m line to an
acoustic release followed by a 12 m riser to a subsurface ﬂoat. The receiver and tag
depths for each range test are summarized in Table 3.2. The range test tags were
attached ∼1.5 m below the subsurface ﬂoat using stainless steel wire, with the tags
positioned out from the mooring line and spaced ∼0.5 m apart to minimize contact
and rubbing. Following the one-year deployment, all moorings were recovered, and
the receiver’s data were downloaded.
Due to the receiver/tag geometry chosen for these experiments, the acoustic signal
from each tag was potentially received at three diﬀerent receivers (except at the
Mid-depth Range test where only two receivers functioned). Therefore, a check was
performed to ensure that each tag was recorded on at least one receiver to conﬁrm
each tag performed as programmed. To minimize the possibility that a given recorded
signal was the result of interference, a detected signal was only considered valid if two
signals from a given tag were received in sequence on the same receiver. Signals which
did not fulﬁl these requirements were removed from further analysis.
For all three range tests, the valid detections were summed daily.
From these
data the detection probability, D, was calculated by dividing the number of daily
detections by the expected number of daily tag transmissions (48 for this study).

43
Detection probability was averaged monthly to allow for seasonal analysis.
Surface conditions and ambient noise
Figure 3.2: (a) Percent ice cover from the Canadian Ice Service weekly charts. (b) The ratio of
the total number of pulses to the total number of synchronizations per day at each site; under ideal
conditions this should be greater than 8. (c) Wind speed and air temperature from NCEP reanalysis
at all three sites.
Due to its high latitude, Cumberland Sound experiences seasonal ice cover (Figure
3.2a). Weekly ice cover proportions from the Canadian Ice Service archives indicate
that the sound had 50% ice cover by 28 November 2011 and 90% by 5 December.
By 19 December 2011, fast ice began forming along the shores of the sound and by
early January 2012, the sound contained primarily shore-fast ice. In May 2012, an
open area formed midway along the south shore and cycled open and closed until the
sound became ice-free. In the same month, at the north end of the sound, areas of
reduced ice cover appeared then closed by early June. Ice began retreating in July
leaving the sound mostly ice-free by late August.

44
Acoustic noise in the water column can potentially mask tag to receiver transmis-
sions and therefore reduce the eﬃciency of a test range or tagging study. Farmer
and Vagle (1988) established that a major component of the ambient noise ﬁeld at
kilohertz frequencies is produced by breaking waves generated by wind.
When the sound was ice-covered, internal ice processes and interactions with the
air and water boundary layers become the primary sources of noise production (Carey
and Evans, 2011). The resulting soundscape can have high variability. The rate of
temperature change, duration and speed of the wind, tidal variations and currents,
and how all these interact with the ice creates additional complexity making it diﬃcult
to estimate what the ambient noise levels might have been during the present study in
the presence of ice. However, Milne et al. (1967) observed that wind and temperature
change were the two main causes of high-frequency noise under ice.
3.2.3
Acoustic Ray-Tracing Model
To investigate and help interpret the acoustic range test data, a simple ray-tracing
model was used. Ray-tracing (e.g., Clay and Medwin, 1977) provides a method to
model sound propagation when the acoustic wavelength is much less than the water
depth, changes in sound speed are negligible over several wavelengths, and the total
ray path is much greater than the wavelength.
The ray-tracing model used was originally developed by Bowlin et al. (1992) then
expanded to include boundary reﬂection losses as well as frequency dependent absorp-
tion by Erbe (2002). In the present simulations the bottom sediment composition was
assumed to be a mixture of sand-silt-clay. Neither bottom scattering or sound propa-
gation through sediment layers are included in the model. At the surface, the model
can be run with either open ocean or solid ice cover losses (for a full discussion see
Erbe (2002)). Sound speed proﬁles, which can be laterally-variable, and bathymetry
are inputs to the model. The model calculates transmission losses between speciﬁed
source and receiver locations. Using the nominal tag source levels (Table 3.1), the
received levels at any receiver can be estimated for diﬀerent sound speed proﬁles,
bathymetries, bottom types and ocean surface scattering.
For this work, the ray tracing model was set-up using the sound speed proﬁle
from 2011 (Figure 4.1b) in 1 m increments and bathymetry from Figure 4.1c in 10
m increments. For each model run 10,000 rays were sent out between -89 and 89
degrees. The model was tested using extreme sound speed proﬁles with sources at

45
diﬀerent heights to ensure rays were refracted as expected.
3.3
Results
Figure 3.3:
Daily detection probability, D, for all three range tests including all tag types in
orange where the colour intensity indicates the number of days at that D value: more days are
darker orange and fewer days are lighter where the aim was to highlight days with lower D. Blue
line is the mean.
Daily detection probabilities, D, deﬁned as the ratio of actual detections over the
number of transmissions, for the three test ranges are shown in Figure 3.3 where
darker circles indicate more data at that value. These results show data varied more
between locations than between diﬀerent tag types. Here, the maximum detection
range is deﬁned as the distance at which the probability of detection drops below 0.5.

46
This range varied with location and tag type. The V16 tags at both the shallow and
deep sites, as well as the shallow V13 were detected over the longest ranges. Days
with signiﬁcantly fewer received tag IDs, or detection drop outs, are common in both
the deep and shallow range tests, especially in the fall of 2011 (Figure 3.3). The
number of observed dropouts is much lower in the mid-depth range test potentially
a result of the overall detection ranges being signiﬁcantly less (Figure 3.3d-f).
Figure 3.4: The mean curves of all received tag signals as a function of range for each test location
(blue lines from Figure 3). Coloured bands indicate one standard deviation from the mean.
The average of all received tags for a single range test are plotted as solid lines
in Figure 3.3. These curves are overlaid on the detection results for each location
and tag type in Figure 3.3, then plotted separately in Figure 3.4 along with ± one
standard deviation. For the Shallow Range Test the ranges at which D drops below

47
0.5 are ∼540 m, >800 m, and 700 m for the V9, V13, and V16 tags, respectively
(Figure 3.4a). This diﬀers from the Mid-Depth study where all 3 tag types have daily
detection probability, D, below 0.5 beyond approximately 250 m (Figure 3.4b). For
the Deep Test Range the D=0.5 ranges are ∼200 m and >800 m for the V13 and
V16 tags (Figure 3.4c).
Figure 3.5: Monthly average range test results for all sites and tags. Thick grey horizontal line
is the D=0.5 detection cut oﬀand the thin, light grey vertical lines are the tag mooring distances
from the receiver.
In both the deep and shallow range tests monthly averaged detections regularly
drop to low values, with signiﬁcant variability between months (Figure 3.5).
For
example, in the shallow V13 test at 661 m detection rates, D, drop below 0.5 only
between August and October, 2011, staying above 0.5 for the rest of the study period
(Figure 3.5b). These drop-outs do not occur at the same range between diﬀerent tags
at each test site. At the shallow site, the biggest drop out for the V16 tags occurred

48
at 727 m (Figure 3.5a), for the V13 tags at 661 m (Figure 3.5b) and for the V9 tags
at 506 m (Figure 3.5c). Similarly, in the Deep Range Test for the V13 tag at 270
m (Figure 3.5h) a drop out occurs without a corresponding drop in V16 receptions
(Figure 3.5g). Possible explanations include: bottom features blocking the sound
(unlikely as no features were detected with the depth sounder), mooring movement
and multi-path signal interference. The lower detection probabilities measured in
July 2012 were likely the result of the batteries failing in the tags.
One of the original objectives of these particular range tests was to evaluate the
eﬀect of water depth on detection range. The results show the most powerful tags
(V16) both in the shallow and deep tests have detection ranges exceeding the maxi-
mum extent of the range test of 800 m; this suggests that the depth of either the tags
or receivers are not an issue within this range, but it is unknown what eﬀect depth
may have at farther ranges. In the following sections each range test is discussed in
more detail.
3.3.1
Shallow Range Test
The Shallow Range Test was conducted at an average depth of 100 m in Pangnirtung
Fjord (Figure 4.1). Since the detailed bathymetry in the fjord is not well known, it is
possible that there may be undocumented obstacles and minor bathymetric features.
However, nothing signiﬁcant was observed on the ship’s echo-sounder while working
in the area. The V16, V13 and V9 tags were tested over ∼800 m and the detection
ranges were good at this site. With a few exceptions, the daily D values for the V16
tags remained above 0.8 out to the maximum extent of the test (807 m) (Figure 3.3a
and 3.5a). At 727 m, D dropped to 0.2 at times between January and July 2012.
In addition, a shorter range detection drop oﬀoccurred in May and early June and
between 300 and 600 m primarily in the autumn of 2011. The V13 average detection
rates were similar to the V16 rates with D>0.8 out to a range of ∼600 m (Figs. 3.3b
and 3.5b). Farther out, the detection probability decreased to ∼0.7 to the maximum
extent of the test. Again, detection drop outs occurred regularly between 550 m and
700 m in the autumn of 2011 and in June-July 2012. Finally, D>0.8 for all V9 tags
closer than 500 m from a receiver (Figures 3.3c and 3.5c). Beyond that distance there
was a signiﬁcant drop in tag detections.

49
3.3.2
Mid-Depth Range Test
At the Mid-depth Range Test location, all three tag types were detected in a similar
pattern (Figures 3.3d-f and 3.5d-f) with the V16 tags showing slightly better perfor-
mance in Figure 3.4b where D is ∼0.4 at 290 m and ∼0.15 at 340 m. By separating
the range test based on whether the acoustic signals had to travel up or down the 10%
slope at the test site, the results shown in Figure 3.3d-f do not change signiﬁcantly.
3.3.3
Deep Range Test
The Deep Range Test was conducted in the deepest part of Cumberland Sound at a
water depth of 1000 m (Figure 4.1). This location is subject to strong tides and likely
experiences occasional inﬂuxes of denser Davis Strait water (Bedard et al., 2015).
Three receivers were positioned 184 m above the bottom while V13 and V16 tags
were deployed 10 m above the bottom, creating the only range test with a signiﬁcant
depth diﬀerence between the tags and receivers. In general, more than 80% of the
V16 tag transmissions were detected out to ∼600 m, but with signiﬁcant variability
as shown in Figure 3.3g and 3.5g. Most of the days with D<0.5 occurred in the
autumn of 2011. At greater ranges the detection probability dropped, but on average
remained above 0.6 out to the maximum range of 802 m.
The deep V13 range test produced the highest variability in D (Figure 3.3h and
3.5h). Also, the detection probability dropped from 0.8 to near 0 at ranges between
200 and 400 m, most easily observed in Figure 3.5h, intermittently over the year. An
example of a signiﬁcant drop followed by a gradual recovery is shown in Figure 3.6
for a tag located at 211 m from the reciever. The detection probability dropped from
close to 1 to near 0 over a 2 day period between 10 and 12 October 2011. Over the
same time frame, D at a range of 354 m, nearly 140 m further out, improved slightly.
This time period corresponds to a time of deep water renewal (Bedard et al., 2015),
when denser water entered the sound from outside and sank to the deepest depths.
These renewal events occurred on spring tides in the fall of 2011. However, there was
not any observed spring-neap cycle in the range test data, suggesting the two events
may be unrelated.
The time intervals between pulses of improved detections (deﬁned as periods when
D>0.1) like those in Figure 3.6 were calculated over the entire year. These pulses
were found to generally last less than 9 hours, with a mean time between them of
days and therefore not related to any tidal frequencies in the sound.

50
Figure 3.6: Detection probability at two ranges for the Deep V13 test. Blue line is for 211 m and
the orange one for 324 m. Dates range between August to December 2011.
3.4
Discussion
The results presented above show both spatial and temporal variability in the de-
tection probability in all three range tests. To facilitate interpretation of acoustic
tag data it is important to understand the factors playing a role in deﬁning the de-
tection probability and its variability. To do this we used a simple two-dimensional
ray-tracing model. Sound energy transmitted by a given tag is inﬂuenced by losses
from spreading and frequency dependent scattering as well as losses due to molecular
absorption as the signal travels to a receiver. The ray tracing model was conﬁgured
with the tag-receiver conﬁgurations of the three range tests and the average of the two
sound speed proﬁles shown in Figure 4.1b. Another factor that may impact results is
focusing/defocusing of rays which is not considered here.
Figure 3.7 shows modelled received level as a function of range for the shallow
range test. The three curves represent the range dependent received level when the
mean source levels of each of the acoustic tags (Table 3.1) are used. From Figure

51
Figure 3.7: Received level as a function of range for the Shallow Range Test where the horizontal
line at 106 dB is the observed detection threshold. Colours denote tag type: Blue for V9, orange for
V13, and green for V16.
3.3a and c it is clear that for the V9 and V16 tags the maximum detection ranges,
or ranges at which D=0.5, are approximately 500 m and 700 m, respectively. Using
these ranges in Figure 3.7 it is possible to estimate that the detection threshold of
the receivers is ∼106 dB re 1 µPa2 Hz−1. Assuming this detection threshold is also
valid for the V13 tags, the results in Figure 3.7 show that the maximum range of the
V13 tags is ∼550 m, which is a shorter range that observed in Figures 3.3, 3.4 and
3.5. Note, according to the tag manufacturer the tags have power outputs that can
vary by as much as 12 dB (Table 3.1), potentially explaining some diﬀerences among
the tag detection probabilities observed in this study.
The acoustic tags transmit coded signals at least 1.82 s in length, composed of eight
69 kHz pulses separated by a blanking period of 260 ms plus a delay associated with
the particular ID. As stated previously, the receivers are programmed not to accept
pulses arriving within the 260 ms blanking period, afterward there is a window when
it is ready to accept the next pulse of the ID code. Ideally, all the pulses along a
given acoustic path will make up the ID code transmitted by a given tag. However,

52
if the signal following another path arrives at the receiver before the next direct path
pulse, it would be recorded and the ID code would be compromised resulting in a
missed detection or mid-range gap or a wrong tag ID recorded. The existence of a
diﬀerent path that could create a mid-range gap depends on the geometry and sound
speed properties of a given site. As a given path becomes longer through reﬂections
oﬀsurface and bottom interfaces, transmission losses due to reﬂection and scattering
increase until the signal is too weak to be detectable at the receiver. Using the ray-
tracing model it is possible to explore and compare the time delay and transmission
losses experienced by diﬀerent rays travelling between a tag and receiver.
Figure 3.8: Model results showing possible paths between the tag and receiver for the four geome-
tries used in this study. (a) Top left is the Shallow Range Test. (b) Top right is the Deep Range
Test. (c) Bottom left is the Mid-depth Range Test up the 10% slope and (d) bottom right is the
same depth but down the 10% slope.
A threshold of 20 dB between the direct path arrival and subsequent arrivals was
set as the limit for detectable signals. i.e., if a secondary path signal was more than
20 dB below the direct path signal, it was discarded.
This threshold was chosen
based on the Shallow Range Test V9 result at a 500 m range (Figure 3.3c) because
this test most closely follows the theoretical curves presented by the manufacturer.

53
Using this threshold, the detection range of the lowest power tag (V9) becomes 500
m (Figure 3.7) which is the same range given by the ‘calm’ setting on the Vemco
range calculator for this tag (https://vemco.com/range-calculator/). Using the 20 dB
threshold, Figure 3.8 shows all possible travel paths between tag and receiver for the
four geometries used in this study, where the tag is at a range of 0 m and the receiver
at the right in the ﬁgures. For the Shallow Range Test (Figure 3.8a) there is the
direct, shortest, path, followed by bottom-reﬂected and surface-reﬂected paths, and
then some source-surface-bottom-surface-receiver paths, etc. The maximum number
of surface reﬂections is 3 and the maximum number of bottom reﬂections is 2. For
the Deep Range Test (Figure 3.8b) there are only the direct path and one surface
reﬂection path that are within our -20 dB criterion. Figure 3.8c and d are the Mid-
Depth Range Tests with 10% up slope and 10% down slope, respectively.
The time delays between the direct path and all the secondary paths for the dif-
ferent tag-receiver combinations are shown in Figure 3.9. In the ﬁgure each diamond
corresponds to the time delay between the direct path and secondary path, and the
solid black horizontal lines show multiples of the 260 ms blank-out time. For the
Shallow Range Test (Figure 3.9a), the pulses following the secondary paths all ar-
rive signiﬁcantly before the 260 ms blanking window. These pulses will therefore not
interfere with the direct path pulses and will not be the cause of dropouts in the
observed data.
The equivalent ﬁgure for the Mid-Depth Range Test (Figure 3.9b) shows the dif-
ference between arrival times for secondary paths and the direct path in the case the
bottom is ﬂat (blue diamonds), 10% downslope (red diamonds), and 10% upslope
(green diamonds). In this case, there is a signiﬁcant opportunity for secondary path
pulses to arrive before the next direct path pulses since the delay is between 250 ms
and 400 ms, with timing especially close to 260 ms at ranges greater than 300 m. We
therefore argue that this interference is likely the cause of the very limited, and tag
independent, detection range for this particular test range.
The Deep Range Test time diﬀerences are shown in Figure 3.9c. Here there are very
few rays following a secondary path that meet the −20 dB requirement. However, the
few that do have delay times close to two pulse detection windows as identiﬁed by the
black line at 520 ms, at ranges greater than 500 m, and it is therefore possible that
these secondary pulses could be detected, especially from the more powerful V16 tags
and therefore help to explain the large variability in the detection probability (Figure
3.3g). It is worth noting that as the surface reﬂectivity increases and decreases due

54
Figure 3.9: Time delays between the direct path and secondary paths shown in Figure 3.8 for all
three range tests taken from Figure 3.7. Black lines indicate the end of the blanking period; the ﬁrst
line is 260 ms, the second 520 ms, the third, 780 ms and the fourth, 1020 ms.
to waves and ice, the signals from any given tag could move into and out of the 20
dB window in which such interference is assumed to be possible.
The ocean is a noisy place where every sound source impacts the noise level (Clay
and Medwin, 1977) and noise in the environment can both improve and reduce the
probability of detecting a tag’s signal. There are many potential sources of noise
at or near the tag frequency, including wind, and rain (Clay and Medwin, 1977).
Additionally, in Arctic environments the constant motion of sea ice creates noise
over various frequency ranges which is transmitted into the water column.
Noise
generated from thermal cracking of ice (Lewis and Denner, 1987) falls in the same

55
frequency range as the tags. However, during this study, we did not observe any
correlation between D and wind speed or sea ice, potentially due to the depths and
short horizontal distances of the range tests. It would be expected that over longer
ranges (i.e. >1 km), and especially for the shallow range test, that surface eﬀects
would have a greater impact.
An example where ambient noise may improve detection is the phenomenon of
Close Proximity Detection Interference (CPDI) (Kessel et al., 2015); a consequence of
multi-path collisions. CPDI creates a minimum eﬀective detection range where, under
the right conditions of low ambient noise and reﬂective boundaries, two paths of the
same signal overlap at the receiver and a detection is not recorded. Although Kessel
et al. (2015) did not discuss it, CPDI also requires a favourable sound speed proﬁle and
the right over-all geometry to have the same signal over two paths arrive at the right
time to overlap. Kessel et al. (2015) showed this eﬀect can lead to a misinterpretation
of acoustical telemetry data and is more of an issue for higher powered tags because
of the greater signal strength resulting in stronger signals following alternate paths.
For example, consider a system with two receivers and a target animal mostly
stationary close to receiver 1 (example from Kessel et al., 2015). If the conditions
were calm, the CPDI eﬀect could be strong resulting in few detection at receiver 1
but many at receiver 2. An interpretation of these results could be that the animal is
spending most of its time near receiver 2. While on a windy day when there is higher
noise levels the CPDI eﬀect would be reduced resulting in more detections at receiver
1 which could be interpreted to mean the animal is moving. This case highlights that
ignoring the eﬀect of CPDI could result in a misinterpretation of how an animal uses
its environment.
All tags will have a maximum eﬀective detection range; they may also have a
minimum eﬀective detection range and potentially regions of detection gaps mid-
range. These gaps along with the minimum and maximum detection ranges may
change as conditions change. The type of tag used does not provide any assurances
mid-range gaps are avoided as these gaps were observed on all tag types used in this
study, including the lowest power V9 tag, and they occurred to depths greater than
1000 m.

56
3.5
Conclusion
In passive acoustic tracking studies, every receiver has a maximum eﬀective detection
range and often a minimum detection range as well. In addition to these constraints,
mid-range detection-gaps may also exist due to multi-path interference. Multi-path
interference occurs when the geometry allows multiple paths of a tag’s transmissions
to overlap. The existence of this interference is an issue that can impact every passive
acoustic telemetry study in some way as it is a function of how ID codes are coded,
speciﬁcally their length, and the geometry combined with environmental conditions
of a study site. In this study, mid-range gaps were observed for all tag types used
including the lowest powered V9 tags, and occurred in water depths greater than
1000 m. It is important to note that these geometries may be diﬀerent in diﬀerent
directions radiating out from the receiver along the horizontal plane. Additionally,
these mid-range gaps, along with the minimum and maximum ranges, may change as
oceanographic conditions change. An example could be when a front such as a salt
wedge moves through the range of a receiver. A tagged ﬁsh on one side of the front
may fall into a detection-gap, while a ﬁsh at the same range on the other side may
not experience the same gap, as sound speed properties may be diﬀerent due to the
diﬀerent conditions. Therefore, range tests may not ﬁnd these gaps unless a tag is
coincidentally deployed at one of these locations. Another important result was that
no seasonality to the detection probability was observed.
Interpreting detection data without knowledge of detection ranges, especially mid-
range gaps could lead to misleading conclusions impacting the validity of any quan-
titative analysis performed. Ultimately, wrong conclusions may be made about how
animals use an area which can potentially lead to inappropriate conservation legisla-
tion. Several options exist to identify and deal with the impact of mid-range detection
gaps. First, tags could be created with coding techniques that reduce the duration
of each ID code, for example, using swept frequencies or phase coding techniques.
Range test tags could be deployed for the duration of the study allowing for identi-
ﬁcation of mid-range detection gaps after a study is complete. Additionally, overlap
between receivers should be designed for an entire study area especially if a quantita-
tive study is planned to determine exact numbers of ﬁsh passing by. Two-dimensional
ray tracing models, although somewhat simplistic, incorporate the factors that cre-
ate multi-path interference. With some basic environmental data (i.e. sound speed
proﬁle and bathymetry), the geometries that most likely will result in multi-path

57
interference can be identiﬁed.

58
Chapter 4
Underwater Soundscape of
Cambridge Bay
A year-long study of the underwater soundscape of Cambridge Bay, Nunavut, was
conducted over 2015. Unlike other Arctic locations considered to date, this site was
louder when covered in ice with the loudest times occurring in April.
Sounds of
anthropogenic origin were found to dominate the soundscape with about ten times
more snowmobile traﬃc on ice than open water boat traﬃc. The bay was quietest
during the ice-break up in July, possibly because it was unsafe for both snowmobiles
and boats. Over the course of the year precipitation, wind and ice noise were the
other major contributors to the underwater soundscape and non-human biological
sources were not signiﬁcant.
4.1
Introduction
The underwater acoustic environment, or soundscape, of a marine habitat inﬂuences
how an ecosystem functions (Staaterman et al., 2013). A soundscape combines envi-
ronmental sounds, biological sounds from the local fauna (Bittencourt et al., 2016), as
well as the sound of humans using the area. Since soundscapes change over multiple
timescales (days, lunar cycles, seasons, etc.) in response to changing environmental
conditions, long-term underwater acoustic recordings can capture natural acoustic
rhythms and expose site-speciﬁc variability (Lillis et al., 2014). In addition, temporal
patterns of marine habitat use can be revealed, including the impact of anthropogenic
acoustic pollution (Rountree et al., 2006; Merchant et al., 2014). In Arctic regions,

59
like Cambridge Bay, ambient sound levels show more seasonal variability than similar
sites in the tropics (Haver et al., 2017).
The physical world creates sound through diverse mechanisms ranging from the
ﬂow noise of tides to the rumbling of earthquakes to meteorological activity. From
above, wind and precipitation can contribute to an underwater soundscape. Wind
noise typically peaks at a frequency around 500 Hz (Wenz, 1962), with higher wind
speeds associated with broadband noise in the range of 0.1-10 kHz (Merchant et al.,
2014). Diﬀerent forms of precipitation on the water surface also add noise to the
underwater environment. The sound of light rain peaks in the 15-25 kHz frequency
band (Nystuen, 1986) while heavier rain generates sound energy down to frequencies
of 500 Hz (Erbe et al., 2015). Both snowfall and rain on open water deposits tiny
bubbles beneath the surface; when these bubbles collapse they emit sound between
50 and 200 kHz and can add up to 30 dB to underwater noise levels (Crum et al.,
1999).
The presence of ice can make a radical diﬀerence to the soundscape through mul-
tiple mechanisms. Sea ice is a dynamic surface in near constant motion (Kinda et al.,
2013) and as it moves sound is transmitted to the water below (Dyer, 1984). Ice
colliding, rubbing, breaking and melting creates sounds over a wide frequency range
from <10 Hz to >10 kHz (Erbe et al., 2015) with two main peaks. At 10-20 Hz, ice
sounds originate from wind blowing over the surface, ridging, and internal fractur-
ing (Dyer, 1984; Greene and Buck, 1977; Pritchard, 1984; Makris and Dyer, 1991).
Sounds between ∼150-5000 Hz are generated from thermal cracking (Milne, 1972).
Sea ice can dampen noise transmitted into the water column resulting in many re-
gions being quieter when covered in ice than when ice free under certain conditions
(for example: Insley et al. (2017)). Another sea ice eﬀect is due to the rough nature
of the underside of the ice which can limit long range sound propagation through the
water column. The consequence of this scattering is that noise at frequencies >1 kHz
is most likely produced locally (Diachok, 1976).
Marine life also contributes to the soundscape. From invertebrates, such as snap-
ping shrimp, to marine mammals, to the more than 700 known species of soniferous
ﬁsh, many underwater animals make noise (Luczkovich et al., 2008). For example,
within a marine protected area in Brazil, ﬁsh choruses eclipsed other sources in the
soundscape (Sanchez-Gendriz and Padovese, 2016) while in a study from the Adriatic
Sea, ﬁsh and snapping shrimp sounds dominated (Pieretti et al., 2017). Migrating
marine mammals can impact season variability of the soundscape; for example, in

60
Fram Strait, another Arctic location, ﬁn whales created a louder environment when
the region was ice free (Haver et al., 2017).
In many environments, anthropogenic noise can be a signiﬁcant component of the
soundscape. Anthropogenic noise can originate from sources located above, below or
on the surface of the water or ice. Below the surface, sonars, explosions, submarines,
remotely operated vehicles, scientiﬁc equipment such as acoustic Doppler current pro-
ﬁlers or pumped conductivity temperature depth instruments, and other subsurface
equipment can all contribute to a soundscape. On the surface, noise can be created
by ships of all sizes by their propellers, engines and other internal workings. Snowmo-
biles or other vehicles operated on top of the ice also contribute. Even aircraft are a
potential noise source in some locations. In the aquatic environment, anthropogenic
noise is nearly ubiquitous and has been shown to cause a variety of negative eﬀects
on the fauna (Williams et al., 2015) by damaging an animal’s hearing and/or forcing
it to change behaviour (Tasker et al., 2010).
Many evaluations of coastal soundscapes have been performed (for example: But-
ler et al. (2016); Lillis et al. (2014); Bittencourt et al. (2016)); however, few have
considered sites with seasonal ice cover (for example: Haver et al. (2017)). This pa-
per provides a description of a year-long, continuous soundscape in Cambridge Bay,
Nunavut, Canada, collected in 2015 using a hydrophone situated on the bottom in ∼9
m of water. This site is a remote Arctic location with a signiﬁcant human presence.
The local people are heavy users of the bay especially when it is ice covered. Here,
we will evaluate how anthropogenic noise contributes to the underwater soundscape
to provide a baseline for future measurements when natural sources or anthropogenic
activities may change. For example, as ice conditions change, the Northwest Passage
running through Dease Strait, at the mouth of Cambridge Bay, may be used more
by shipping, potentially adding more summer noise to the bay’s soundscape. To our
knowledge, this is the ﬁrst study looking at the impact of traﬃc both on ice and over
open water in a remote Arctic location, as well as, perhaps, the ﬁrst discussion of the
underwater noise generated by snowmobiles.

61
Figure 4.1: Map of Cambridge Bay where the community of the same name is highlighted in
purple and the location of the underwater platform with a red dot. Bathymetry is taken from Gade
et al. (1974). Inset map is of northern Canada with the location of Cambridge Bay denoted with a
red square.
4.2
Methods
4.2.1
Location
Cambridge Bay, Nunavut, Canada (69.11◦N, 105.05◦W) is an Arctic location where
an Ocean Networks Canada (ONC) cabled platform is located (Figure 4.1). The bay
cuts into the southern side of Victoria Island (inset map of Figure 4.1) oﬀDease Strait
with the community of Cambridge Bay located on its shore. The bay is part of the
Canadian Arctic Archipelago (CAA), a region of islands and narrow straits where
Paciﬁc origin water passes through to the Atlantic (Prinsenberg and Bennett, 1989;
McLaughlin et al., 2004; Rudels, 2012). The complex bathymetry of the bay can be
simpliﬁed into two basins separated from Dease Strait by an 11 m sill. The outer
basin has a maximum depth of 31 m, and a 20 m sill cuts oﬀthe inner basin. The
focus of this study will be on the farthest inland section of the bay. The maximum
depth of the eastern part is 86 m; it reaches 48 m in the centre and a maximum depth

62
of 57 m in the western part.
Environmental conditions at the study site are typical for an Arctic location. Ex-
treme annual variations in light levels combine with landfast ice that blankets the
bay over the winter, reaching a maximum thickness of 2 m (McLaughlin et al., 2004).
The meteorology in the western CAA, where Cambridge Bay is located, is similar to
that found in the Beaufort Sea and varies with large-scale atmospheric ﬂuctuations of
the Arctic Oscillation (Barber and Hanesiak, 2004). In this region semi-diural tides
have an average range of 0.4 m (Gade et al., 1974).
Gade et al. (1974) found that the heat budget of the bay was nearly locally balanced
with little net exchange with outside water. The outside water in Dease Strait and
Coronation Gulf are heavily inﬂuenced by the outﬂow of the MacKenzie River (Tully,
1952), which does not extend into the bay. Seasonal meltwater rivers discharge large
amounts of fresh water to the bay over the short summer season, then are frozen over
the rest of the year (McLaughlin et al., 2004). Salt exchanges are limited to summer
months with the salt outﬂow due to entrainment with runoﬀbeing roughly balanced
by an inﬂow at depth occurring after runoﬀceases. During winter, the bay is virtually
shut oﬀfrom any fresh water discharge. As a result, estuarine circulation stops and
bay circulation is governed by other processes, such as convection and tides.
Within Cambridge Bay, two systems of convective circulation were found by Gade
et al. (1974). Salt rejection at the ice-water interface creates a homogenous upper
layer under the ice. Deeper, the conditions become more complex. Where the water is
shallow the upper layer convection reaches the bottom and the restricted circulation
allows for an accumulation of rejected salty water. As a result, the density of water
in the shallows exceeds the density in deeper waters and this salty, dense water ﬂows
down the slopes (Gade et al., 1974).
4.2.2
Environmental Data
All data used in this study were collected using ONC’s instruments.
The cabled
platform was located at a depth of ∼9 m at a distance of 78 m from the shore at
69.1133◦N, and 105.060◦W (Figure 4.1). A Sea-Bird SeaCAT SBE19plus V2 Con-
ductivity Temperature Depth (CTD) probe was located on the underwater platform
and sampled at 1 Hz. On the same platform was an ASL Shallow Water Ice Proﬁler
(SWIP), an upward-looking active sonar designed to measure ice draft in shallow
water environments. Both instruments were exchanged for diﬀerent ones of the same

63
Serial
Latitude
Longitude
Depth
Deployment Dates
Number
[N]
[W]
[m]
1288
69.11342
105.06129
8
1 January 2015 to 26 August 2015
1252
69.11268
105.06490
13
27 August 2015 to 31 December 2015
Table 4.1: Ocean Sonics icListen HF Hydrophone deployments and locations in Cambridge Bay
covering 2015.
make and settings in late August 2015. Meteorological parameters were measured
by a Luﬀt WS501 Weather Station located on shore (at 69.1139◦N, and 105.060◦W).
This instrument made measurements every 60 s and remained in place for all of 2015.
4.2.3
Acoustic Recordings
Two hydrophone deployments occurred in 2015 (Table 4.1), and both hydrophones
were calibrated by Ocean Networks Canada prior to deployment. Since the platform
containing the hydrophones also contained other instruments, platform noise mani-
fested as tonals at various frequencies which changed between deployments. Acoustic
data were recorded continuously as wav ﬁles over frequencies up to 27.5 kHz for the
ﬁrst deployment and up to 8 kHz for the second at a sample rate of 64000 Hz. Spectra
were computed using a Hann window with 50% overlap with 6400 samples per FFT
window. All sound level measurements are reported as spectral levels in dB re 1 µPa2
Hz−1.
A number of data gaps occurred over the course of 2015. To quantify these gaps,
the amount of data recorded per month is given in Table 4.2. From January to June
most of the expected data were recorded, on average >95%. Rates dropped over the
summer (July-August) to 60%. In September, less than 1% of the expected acoustic
data were recorded (only about 8 hrs); therefore, this month will be omitted from
further analysis. In November and December, recording rates increased again to an
average of 86%. Unexpected power outages were generally responsible for the data
gaps; the exception was a planned power outage for platform maintenance performed
late August.

64
Month
Total number of ﬁles
Expected number of ﬁles
Percent data
January
8916
8928
>99
Febuary
7136
8064
95
March
8517
8928
95
April
8641
8640
>99
May
8214
8928
92
June
8638
8640
>99
July
5460
8928
61
August
5419
8928
61
September
37
8640
<1
October
4958
8928
56
November
7658
8640
87
December
7586
8928
85
Table 4.2: Expected and received data by month, where each ﬁle is ﬁve minutes in duration. The
percentage of available data is included in the ﬁnal column.
4.3
Results
4.3.1
Environmental Conditions
Over the course of the year, Cambridge Bay experienced extremes in light levels and
temperature (Figure 4.2a and b) consistent with an Arctic environment. Global radi-
ation ranged from zero during times of complete darkness in January and December,
2015, to >800 W m−2 around the summer solstice when daylight was continuous. Air
temperature had an ∼60 ◦C range, from −40 ◦C in winter to 20 ◦C in summer (Fig-
ure 4.2b). Temperatures averaged below −20 ◦C from January into April, then again
in October to the end of the year. The coldest temperatures were recorded mid-
January through mid-February and temperatures only ranged above freezing May
through September and into October. The warmest temperatures occurred in July
and August.
Cambridge Bay has seasonal ice cover (Figure 4.2 and Figure 4.3a). In 2015, ice
reached its maximum thickness of 1.78 m in mid-May, consistent with the thickness
range of ﬁrst year ice found in the Canadian Arctic Archipelago (McLaughlin et al.,
2005).
Over the next month, ice thickness rapidly decreased resulting in ice free
conditions by the end of June. From mid-July to mid-October, the bay remained ice
free. By mid-October ice began to form, increasing in thickness to the end of the
year.

65
Figure 4.2: Environmental conditions in Cambridge Bay over 2015.
The shore-based weather
station recorded global radiation (a) and air temperature (b). Water temperature (c), practical
salinity (d) and sound speed (e) are from the CTD on the underwater platform. Fifteen minute
averaging was performed on all data and the ice free period from the SWIP is highlighted in yellow.
When the bay was covered in ice, water temperature remained at about −1.6
◦C and salinity ranged between 28-29 PSU (Figure 4.2c and d).
Under the ice,
the sound speed hovered around ∼1434 m s−1 (Figure 4.2e). When there was open
water in the bay, both the water temperature and salinity had much wider ranges.
Water temperature increased, peaking at 8.7 ◦C in August (Figure 4.2c) and salinity
decreased to a minimum of 13.6 PSU (Figure 4.2d).
Sound speed was also most
variable during ice-free times, reaching a maximum of 1465 m s−1 early August (Figure
4.2e). The average windspeed was ∼5 m s−1 (Figure 4.3c). Events with wind speeds
approaching 20 m s−1 occurred in January, October and November. Although wind
direction varied, the winds tended to be southeasterly especially in January, February,
March and October.

66
Figure 4.3: (a) Ice thickness in metres from the SWIP ice proﬁler. (b) Year long spectrogram
created from the acoustic recordings with one hour averaging and 500 Hz frequency bins; colour bar
denotes sound power intensity in dB re 1 µPa2 Hz−1. (c) Wind speed from the weather station.
4.3.2
Underwater Acoustic Environment
To characterize the soundscape of Cambridge Bay, the acoustic data are presented to
highlight broad features ﬁrst, then focus on details.
The power spectral density as a function of time for 2015 is shown in Figure 4.3b
using one hour averaging and 500 Hz frequency bin size. Until August, the maximum
frequency recorded was 27.5 kHz. During this time there was a persistent tonal at ∼21
kHz likely caused by one of the other instruments co-located on the same platform.
Unfortunately, when the hydrophone was replaced in August the maximum frequency
recorded was reduced to 8 kHz for unknown reasons. To gain a better understanding

67
Figure 4.4: (a) Power spectral density in three frequency bands for 15-hour averages (thin lines)
presented along with the monthly average of the band (thicker line). (b) Power spectral density with
the 5th percentile of the spectral probability density of the quietest month (July 2015) removed over
the full range of frequencies.
of the relative contribution of diﬀerent frequency bands to the overall soundscape,
the yearly-spectrogram was broken into three bands, 0-1, 1-10 and 10-25 kHz. Only
the lowest frequency band was computed in the fall with the new, lower bandwidth.
Each frequency band was averaged over 15 hours, then averaged over a calendar
month (Figure 4.4a).
To present the noise level distribution over the full range of the recordings, spectral
probability densities (SPD) were computed using normalized histograms over the full
frequency range in 1-Hz intervals. Next, statistical variability was calculated and split
into three power spectral density percentile bands where the nth percentile gives the
level that was lower n% of the time (Figure 4.5). The 50th percentile is the median
(Figure 4.5b). The ∼21 kHz tonal identiﬁed in the year-long spectrogram showed up

68
Figure 4.5: Monthly percentiles calculated from the spectral probability densities. September is
greyed out as there were only 8 hours of data recorded that month.
as a spike in the SPD analysis (Figure 4.5). To look at how much louder the rest
of the year was compared to the quietest month (July 2015), the 5th percentile of
the SPD over the full range of frequencies was removed from the entire year and the
remainder was plotted as a spectrogram (Figure 4.4b).
Over all frequencies, this analysis highlights that July was the quietest month
and April was the loudest (Figure 4.4a). Below 1 kHz, ambient sound levels averaged
around 70 dB reaching 100-110 dB at times (Figure 4.4a). This band was consistently
louder than the higher frequencies (Figure 4.4a), peaking in April. The second loudest
months were January and August. The 1-10 kHz band was next loudest, on average
just below 60 dB, except during June and July when noise in this band dropped to
∼45 dB which was quieter than the highest band. The loudest times in the middle

69
band were the same as those for the lower band. The highest band was more consistent
throughout the year with monthly averages ranging from 47-50 dB. Peaks occurred
in April, August and January. In June, July and August 2015 there were harmonics
around 17 and 24 kHz, likely originating from a 50 kHz sonar that started 29 May
onboard a tug boat tied up at a near by dock. These harmonics are relatively quiet,
reaching at most 50 dB.
A quiet period over the entire frequency range started in June (Figure 4.4a) cor-
responding to ice break-up. As noted above, July was the quietest month and also
the ﬁrst ice-free month of the year. During this quiet time, wind speeds did not
exceed 10 m s−1 (Figure 4.3c). This quiet period could also have occurred because
ice break up makes the bay dangerous for human transport, reducing the amount of
anthropogenic noise, an issue which will be considered in the discussion. In addition
to noise generated by the wind, the ice-free time included noise from the nearby dock
from wave action and boats rubbing. There was also boat noise from engines and
onboard equipment such at generators, pumps and sonars. A sub-set of a acoustic
ﬁles were listened to. Occasional ﬁsh grunts were heard but, no marine mammal calls
were identiﬁed.
4.4
Discussion
Simultaneous contributions from many sources make the underwater soundscape of
Cambridge Bay complex. The most relevant sources are considered below.
4.4.1
Biological Sounds
Occasional ﬁsh sounds in the form of grunts were observed at frequencies between
120-380 Hz. However, it is unknown what species was calling, nor did the calls make
a signiﬁcant contribution to the soundscape. Over the course of 2015, no marine
mammal calls were identiﬁed through either listening to the acoustic ﬁles or examining
the spectrograms. This does not mean marine mammals were not present; however,
their contribution to the soundscape was minimal.
Overall, biological sources in
Cambridge Bay were not a signiﬁcant contributor to the underwater soundscape.

70
4.4.2
Physical Process Sounds
Of all the local physical processes, precipitation, wind and ice noise were the most sig-
niﬁcant sources. Tidal currents are another potential noise generating process (Urick,
1983); however, in Cambridge Bay, the noise generated was small likely because the
tidal range was only 0.4 m; thus, tides were not considered as a major contributor to
the soundscape.
Precipitation
Month
Precipitation [cm]
January
no data
Febuary
0.34
March
0.28
April
0.28
May
0.36
June
2.54
July
5.94
August
1.18
September
1.10
October
1.22
November
1.18
December
0.46
Table 4.3: Monthly precipitation for 2015 from http://climate.weather.gc.ca. Note, no data was
available for January 2015.
To consider the contribution of precipitation to the soundscape of Cambridge Bay,
data from the Government of Canada historical climate data repository was used as
summarized in Table 4.3. Over 2015 there was ∼15 cm of precipitation. Note, no
data was available for January 2015; however, it was likely low like the other winter
months. Assuming snowfall on ice does not transfer much noise into the water column
and that there was no rainfall on the ice, the period of open water would be the time
precipitation would contribute the most sound. During ice-free times, precipitation
would be expected to create a broad signal over a wide range of frequencies with a
peak between 15-25 kHz (Nystuen, 1986). Over the ice-free period of 2015 a total of
10 cm of precipitation fell, with the most during a single day being <2 cm. From a
visual inspection of the data, precipitation events were found to be intermittent; an

71
Figure 4.6: A ﬁve minute spectrogram from 15 August, 2015. The two intermittent rain events
are highlighted with red boxes. The events peak between ∼14 and 15 kHz and reach at most 80 dB
re 1 µPa2 Hz−1.
example is shown in Figure 4.6. This event reached a maximum of 62 dB re 1 µPa2
Hz−1 at 15 kHz.
Wind noise
Wind is potentially a major contributor to the soundscape. Acoustical noise is trans-
mitted into the water column by wind-generated breaking waves when there is no
ice cover and through wind induced ice movement and blowing snow when there is
ice cover. Alternately, the ice may act to insulate the water column from wind noise
resulting in decreased noise (Greene, 1995; Insley et al., 2017). Here, wind noise will
ﬁrst be considered separate from the ice cover.
Wenz (1962) found that in shallow, open water wind generated noise can be loud,
ranging from 80 dB re 1 µPa2 Hz−1 up to 140 dB re 1 µPa2 Hz−1 and occurs mostly
a low frequencies, i.e. below 500 Hz. Thus the majority of wind noise is expected to
fall in the lowest frequency band of up to 1000 Hz in Figure 4.4a. With the exception

72
of April, throughout the year this band averages just above 70 dB re 1 µPa2 Hz−1
with some daily spikes up to 110 dB re 1 µPa2 Hz−1. During the ice-breakup in June
and ice-free period of July, sound in the lowest band dropped to the lowest levels of
the year of below 65 dB re 1 µPa2 Hz−1.
Figure 4.7: Winds presented by month in a wind rose format. The rings denote increasing per-
centage of the time the wind blew in that direction. From the inside going outward, the rings are 5,
10, 15, 20%. Colours denote wind speed in m s−1.
Winds presented in a monthly wind rose format in Figure 4.7 show wind speed and
direction each month. The lowest monthly average in the 0-1000 Hz frequency band
occurred in July; this month had winds which averaged ∼5 m s−1 which is the average
wind speed of the year.
However, July winds were predominantly northeastward
which was an anomalous direction for the year (Figure 4.7). This was also the ﬁrst
full ice-free month and that likely also played a role in the low noise level. The lowest

73
frequency band was loudest in April (Figure 4.4a), which was not a month with
exceptionally high winds suggesting another source contributed to the soundscape at
this time.
The strongest winds occurred in January, February, March and October (Figure
4.7), all months with ice cover. In early March, after 15-minute average wind speeds
exceeded 15 m s−1 (Figure 4.3c) noise in the 0-1000 Hz band peaked at 110 dB re 1
µPa2 Hz−1, the loudest period of the year (Figure 4.4a) suggesting this wind event
signiﬁcantly increased underwater noise.
To quantify the amount of noise added to the water column by wind, a statistical
analysis of the relationship between surface winds and the observed noise ﬁeld devel-
oped by Vagle et al. (1990) was used (Figure 4.8). This approach relates the sound
spectrum level, SSL(f), at frequency f in Hz, to the wind speed, U10, in m s−1, at an
elevation of 10 m, as:
SSL(f) = 20 log10(sU10 + b) −Q log10(8000/f)
(4.1)
In the equation Q is a unitless parameter incorporating how the SSL decreases as
the frequency increases: here Q=−21 is used based on the slope of the average PSD
over the year between 1 and 10 kHz. Parameters s and b are the slope and intercept
of a linear ﬁt between measured sound pressure and windspeed with s=58.87 and
b=−80.94 used from Vagle et al. (1990).
Since the hydrophone was shallow, it is assumed there is negligible attenuation loss.
This calculation was performed at 3 kHz. During the quietest month, July, noise
measured was, in general, only slightly higher than the calculated wind-generated
noise. This suggests wind generated noise dominated the soundscape at this time. In
August, the measured sound ranged much higher, reaching above 80 dB re 1 µPa2
Hz−1 at times. The calculated wind noise during this period did not exceed ∼65 dB
re 1 µPa2 Hz−1 suggesting other sources contributed noise in addition to the wind
such as noise from the boats tied up at the nearby dock.
Ice Noise
As the stresses on sea ice change, noise can be generated and transmitted to the
water column below. Based on the proximity to shore, the assumption will be made
that the ice cover on Cambridge Bay is land fast. To relate the soundscape to ice
cover, ice thickness is plotted in Figure 4.3a. A noise peak has been reported for

74
Figure 4.8: Daily average wind contribution to ambient noise calculated at 3 kHz based on averaged
wind speed for the ice free period in blue. Red is the daily average in the 3 kHz frequency band.
ice-covered environments around 10-20 Hz from wind blowing over ice, ridging, ice
cracking, and bending (Dyer, 1984; Greene and Buck, 1977; Pritchard, 1984; Makris
and Dyer, 1991). By far, the lowest band (which includes 10-20 Hz) in Cambridge
Bay is the noisiest; however, it retains similar noise levels during times of no ice. One
major diﬀerence between the two time short-term variability, with times of ice cover
being more variable most likely due to ice-related noise.
Date
Wind
Air
24 Hour
Number of
Speed
Temperature
Temperature
Cracks
[m s−1]
[◦C]
Change [◦C]
28 January 2015
10.2
−31.7
4.8
54
3 February 2015
3.27
−34
4.3
1
1 April 2015
6
−31
8.2
184
Table 4.4: Environmental parameters and the number of cracks over three 5-minute samples. For
all sample times, water temperature remained relatively stable between −1.5 and −1.6 ◦C.

75
Figure 4.9: Ice cracking from 1 April 2015. Top panel is the waveform where the ice cracking
manifests as spikes. The bottom panel is a ﬁve minute spectrogram where the PSD of the short ice
cracking events reached a maximum of 87 dB re 1 µPa2 Hz−1 at 5 kHz.
Thermal cracking of ice adds sound to the water column with a noise peak between
150-5000 Hz (Milne, 1972) as a series of short pulses where each pulse sounds like a
‘pop’. Table 4.4 lists three 5-minute times where ice cracks were counted, together
with the environmental conditions during these periods. An example of ice cracking
from 1 April 2015 is shown in Figure 4.9 where a manual count found 184 cracks
during the 5-minute period. For all three times, air and water temperatures were
very similar; however, the change in air temperature over the 24 hours around the
time with the most cracks on 1 April 2015 was 8.2 ◦C, almost double the other values
presented in Table 4.4. The lowest windspeed on 3 February 2015 coincided with
almost no cracking; although winds were stronger on 28 January 2015 than on 1
April 2015, this did not result in more cracking, suggesting the increased number in

76
cracks were more related to temperature change, although this is an extremely small
set of samples where multiple factors were in play.
4.4.3
Anthropogenic Sound
Vehicle noise dominated the anthropogenic component of the soundscape in Cam-
bridge Bay, with ice cover being the deciding factor between boat or snowmobile use.
Traﬃc noise in open shallow water typically falls in the frequency band between 10-
1000 Hz and ranges from below 40 dB re 1 µPa2 Hz−1 to as high as 100 dB re 1 µPa2
Hz−1 (Wenz, 1962); this was also found to be true at Cambridge Bay for both open
water and ice-covered conditions.
Figure 4.10: A typical underwater noise signature of a small boat as observed on 15 July 2015.
Top panel shows the raw pressure signal of the passing boat. The bottom panel is a ﬁve minute
spectrogram where the boat sounds reached a maximum of 145 dB re 1 µPa2 Hz−1.

77
During ice free times, boats generate sounds over a wide band of frequencies as a
result of engine operation and propeller blade rotation (Figure 4.10). Typically, the
boats in Cambridge Bay were small ﬁshing vessels and pleasure craft. An additional
noise contributor was a Norwegian tug boat tied along the dock a short distance from
the hydrophone. At ∼23:59 hrs UTC on 28 May 2015 a 50 kHz signal began and
continued on until the fall (it is diﬃcult to determine an exact end due to the data
gaps in September). Several, less powerful, harmonics of this signal can be observed
down to ∼17 kHz (Figure 4.3b and 4.4b).
Figure 4.11: Underwater noise signatures from two snowmobiles observed on 1 January 2015.
Top panel shows the raw pressure signal in arbitrary units. The bottom panel shows a ﬁve minute
spectrogram where the snowmobile PSD reached 152 dB re µPa2 Hz−1.
Snowmobiles were operated when there was ice cover. These vehicles have similar
combustion engine sounds as the small boats combined with the sound of the track
and skis moving across the surface (Figure 4.11). It is possible a small portion of the

78
vehicle traﬃc on the ice is other types of transportation such as trucks.
Figure 4.12: Top is the ice thickness on Cambridge Bay. Bottom are bars for the average number
of vehicle passages (snowmobile or boat, depending on ice cover) counted over seven days of each
month. Black lines are the error bars. Monthly frequency bands from 4.4a are included layered on
top of the bottom panel with scale on the right side.
To make an assessment of how much vehicle traﬃc contributes to underwater noise
in the bay, seven days were selected each month and analyzed manually. Lloyd’s
mirror interference patterns (for example see Figures 4.10 and 4.11) were counted
where noise levels exceeded 90 dB re 1 µPa2 Hz−1, and the results were averaged
together (Figure 4.12). Note, this method counts the same vehicle more than once if
it passed over the hydrophone multiple times and the error is greater when multiple
vehicles pass over close together as their signals merge potentially resulting in a lower
estimate.
On the ice, traﬃc increased each month until reaching a peak in April (Figure
4.12).
On a single Saturday that month >350 vehicle passages were counted; on

79
average 100 more vehicles passed over the hydrophone than in either March or May.
The signiﬁcantly increased noise in the lowest frequency band (0-1000 Hz) in April
(Figure 4.4a and 4.12) has already been shown not to be a direct result of winds. The
increased vehicle traﬃc that month is almost certainly the source of the additional
noise. In May, traﬃc levels were equivalent to those of April for the ﬁrst part of the
month, then dropped oﬀas ice break up began, likely due to the dangerous conditions.
The open water of June and July saw few boats. A possible reason for the quiet
months of June and July (Figure 4.4a), especially during the ice break up, could be
reduced traﬃc on the bay (Figure 4.12). Boat numbers increased dramatically in
August to a maximum >80 in a single day. However, our results show that there
were ten times the number of snowmobile passages than boats. These results suggest
that sounds of anthropogenic origin, speciﬁcally vehicles, dominate the soundscape
of Cambridge Bay, especially when there is ice cover.
4.4.4
Relative contributions of diﬀerent sources
To characterize the contribution of the various sources discussed above, the major
components of the soundscape in Cambridge Bay are presented in Figure 4.13. A
baseline noise spectrum (purple line in Figure 4.13) was chosen from an ice-covered
period with little wind and ice cracking. A ﬁve minute averaged noise spectrum from
a period that includes the precipitation event shown in Figure 6 is plotted in green in
Figure 4.13. These are both relatively quiet periods that have similar characteristics at
mid-frequencies between 0.3 and 10 kHz. However, the ice-covered baseline spectrum
is higher at the lowest frequencies, peaking at 60 Hz, than the ice-free time with
precipitation. Above 10 kHz precipitation adds more noise.
Ice cracking added a moderate amount of noise into the water column during times
of ice cover. Here, the period with ice-cracking (blue line in Figure 4.13) occurred
1 April 2015, when 184 cracks were recorded over a ﬁve minute period (Table 4.4).
This period was louder than the ice-covered baseline period, peaking at ∼90 dB re
1 µPa2 Hz−1 at 60 Hz. Note, the ice-covered baseline spectrum also peaked around
the same value, suggesting another ice-related source of the sound at this frequency.
In the frequency band between 0.3-10 kHz, ice-cracking was ∼20 dB louder than the
baseline and precipitation periods.
The contributions from both boats and snowmobiles were similar and, by far, the
loudest contributors to the soundscape. Although these sources were intermittent, at

80
Figure 4.13: Relative contribution by frequency of the major contributors to Cambridge Bay’s
soundscape.
Both the snowmobiles and boat contributions are taken from the closest point of
approach of three vehicles averaged together. All others are an average over ﬁve minutes.
the closest point of approach both boats and snowmobiles had PSDs above 100 dB re
1 µPa2 Hz−1 at frequencies up to 3 kHz. Above 3 kHz the noise levels begin to drop,
but still remain above all the other major contributors to the soundscape. At times
these PSDs exceeded 120 dB re 1 µPa2 Hz−1. The number of vehicles at diﬀerent
times (Figure 4.11) further conﬁrmed that these sources dominated the soundscape
at diﬀerent times of the year especially when there was ice cover.
4.4.5
Comparison to other Arctic soundscapes
Shallow water soundscapes have been considered in many studies but none were
found discussing the sound contribution of the passages of snowmobiles on the ice
on the soundscape below. Furthermore, before the soundscape of Cambridge Bay
can be compared to some of these other sites it is worth noting that environmental
conditions, anthropogenic sources and site geometry can be vastly diﬀerent between
sites (Haxel et al., 2013).

81
Here we will consider the soundscapes of two other Arctic sites. In a study con-
ducted near Sachs Harbour, ∼800 km from Cambridge Bay, ambient sound levels were
lower during ice cover than during periods without ice. During January to March at
this site noise levels dipped below the noise ﬂoor of the hydrophones used, that is,
<70 ±10 dB re 1 µPa2 Hz−1 (Insley et al., 2017). In the Beaufort Sea, under-ice noise
was found to be ∼65 ±10 dB re 1 µPa2 Hz−1 at a frequency of 100 Hz (Kinda et al.,
2013). In Cambridge Bay, noise at this frequency was >80 dB re 1 µPa2 Hz−1 for
both periods with signiﬁcant ice cracking and periods with no ice cracking (Figure
4.13 blue and purple spectra) suggesting Cambridge Bay is overall a noisier site.
An acoustic survey of another Arctic location with seasonal ice cover, Fram Strait,
was conducted in 2009-2010 (Haver et al., 2017). In general, this site was louder than
Cambridge Bay, with median broadband sounds exceeding 100 dB re 1 µPa2 Hz−1.
Like Cambridge Bay, Fram Strait experiences strong seasonal variability, but was
found to be quieter in winter than in summer (Haver et al., 2017). In Fram Strait,
biological sources, speciﬁcally ﬁn whales, were the major reason for the seasonal
variability. Although, marine mammals were not observed during the ice free period
in Cambridge Bay, the biggest reason for the diﬀerence is likely the heavy use of
snowmobiles.
The anthropogenic source of much of the sound in Cambridge Bay
suggests this site may have more in common with heavily traﬃcked sites in coastal
areas despite the high latitude.
Another component that has not been captured in the current soundscape of Cam-
bridge Bay, but is an issue in more southern locations (for example: Garrett et al.
(2016)) is the sound of distant shipping. Diachok (1976) noted that scattering under
ice prevents sound from propagating long distances, suggesting this is more likely to
be present when there is open water. The nearby Dease Strait (Figure 4.1) provides
a thoroughfare for transiting the Northwest Passage; as climate change reduces the
amount of ice cover, this route may be used more, potentially adding more summer
noise to the Cambridge Bay soundscape.
4.5
Conclusion
This may be the ﬁrst study showing the contribution of snowmobiles to a marine
soundscape. In 2015, the soundscape in Cambridge Bay was dominated by anthro-
pogenic noise, which was louder when there was ice cover than during ice free periods.
This seasonal variability was a signiﬁcant factor as the bay was quietest during the

82
ice-break up in July possibly because it was unsafe for both snowmobiles and boats.
The ambient sound levels in the bay were driven by ice cover through controlling the
types of vehicles using the bay (snowmobiles on ice and boats on open water). Al-
though the observed source levels of both boats and snowmobiles were similar, in 2015
there were in total ten times more snowmobile passages than boats. The abundance
of snowmobiles passages relative to boats explains why the bay was louder when there
was ice cover.
Over the course of the year, precipitation, wind and ice noise were other important
contributors to the underwater soundscape. Biological noise was not a signiﬁcant
sound source and the relationship between wildlife presence and anthropogenic sounds
is unknown.

83
Chapter 5
Conclusion
The objective of this thesis was to gain an understanding of the link between Arctic
coastal oceanography and the local soundscape on the detection range of acoustically-
tagged ﬁsh to give context to biological behaviour recorded. The intent was to do
this through determining what oceanographic processes deﬁne the underwater envi-
ronment and what sources dominate the soundscape. Then, by looking at how these
physical processes and sounds impact tag eﬀectiveness within this environment.
Here, we explored the oceanographic processes within the Arctic coastal embay-
ment of Cumberland Sound, then evaluated the functionality of passive acoustic ﬁsh
tracking tags in use there. In the sound, both oceanographic, and acoustic tag range
data were collected in situ, and combined with a simple ray-tracing model. Due to
reasons external to this project, it became impossible to obtain acoustic recording
in Cumberland Sound, so the underwater soundscape of another Arctic coastal site,
Cambridge Bay was described.
The following sections highlight the important results.
5.1
Oceanography of Cumberland Sound
Cumberland Sound is a large embayment on the southeast coast of Baﬃn Island that
opens to Davis Strait. The ﬁsh population thriving at the deepest depths demonstrate
that hypoxic conditions do not exist. Conductivity and temperature verses depth pro-
ﬁles were collected during three summer ﬁeld seasons (2011-2013) and two moorings
were deployed during 2011-2012. Within the sound, salinity increased with increasing
depth while water temperature cooled reaching a minimum at roughly 100 m. Below

84
100 m, the water became both warmer and saltier. Temperature-salinity curves for
each year followed a similar pattern, but the entire water column in Cumberland
Sound cooled from 2011 to 2012, then warmed through the summer of 2013.
Even though the sound’s maximum depth is over a kilometre deeper than its sill,
water in the entire sound is well oxygenated. A comparison of water masses within
the sound and in Davis Strait shows that, above the sill, the sound is ﬂooded with
cold Baﬃn Island Current water following an intermittent geostrophic ﬂow pattern
entering the sound along the north coast and leaving along the south. Below the sill,
replenishment is infrequent and includes water from both the Baﬃn Island Current
and the West Greenland Current. Deep water replenishment occurred more frequently
on spring tides, especially in the fall of 2011. Although the sound’s circulation is
controlled by outside currents, internal water modifying processes are presumed to
occur such as estuarine ﬂow and wind-driven mixing.
5.2
Detection Range Variability of Passive Acous-
tic Tags
Passive acoustic tracking is a technique gaining in popularly to quantify ﬁsh move-
ment and is being used to make ﬁshery management decisions. Typically, research
using this method focuses on detections without fully considering the inﬂuence of the
environment. In Cumberland Sound there are three species of deep-water ﬁsh being
currently tracked. Detection ranges obtained through a series of year-long acoustic
functionality experiments (range tests) were combined with two-dimensional ray trac-
ing model results to examine the eﬀect of environmental factors on detection ranges.
Multi-path signal interference emerged as a major issue interrupting the continuity
of detection ranges.
Every receiver has a maximum eﬀective detection range and often a minimum
detection range as well. In addition to these constraints, mid-range detection-gaps
may also exist due to multi-path interference. Multi-path interference occurs when
the geometry allows multiple paths of a tag’s transmissions to overlap. The existence
of this interference is an issue that can impact every passive acoustic telemetry study
in some way as it is a function of how ID are coded, speciﬁcally their length, and the
geometry combined with environmental conditions of a study site.
Mid-range gaps were observed for all tag types used in this study. It is important to

85
note that site geometry may be diﬀerent in diﬀerent directions radiating out from the
receiver along the horizontal plane. Additionally, these mid-range gaps, along with
the minimum and maximum ranges, may change as oceanographic conditions change.
Therefore, range tests may not ﬁnd these gaps unless a tag is coincidentally deployed
at one of these locations. Interpreting detection data without knowledge of detection
ranges, especially mid-range gaps could lead to misleading conclusions impacting the
validity of any quantitative analysis performed. Ultimately, wrong conclusions may
be made about how animals use an area which can potentially lead to inappropriate
conservation legislation.
Several options exist to identify and deal with the impact of mid-range detec-
tion gaps. Range test tags could be deployed for the duration of the study allowing
for identiﬁcation of mid-range detection gaps after a study is complete. Addition-
ally, overlap between receivers should be designed for an entire study area especially
if a quantitative study is planned to determine numbers of ﬁsh passing by. Two-
dimensional ray tracing models, although somewhat simplistic, incorporate the fac-
tors that create multi-path interference. With some basic environmental data (i.e.
sound speed proﬁle and bathymetry), the geometries most likely to result in multi-
path interference can be identiﬁed. A reasonable ﬁrst step to identify if multi-path
interference may be an issue at a site, would be to assume minimal refraction over
the range test and calculate travel times for diﬀerent paths based on straight lines.
5.3
Cambridge Bay Soundscape
Finally, the marine soundscape in Cambridge Bay was evaluated over 2015 and it
was found that noise of anthropogenic origin dominated. The site was louder when
there was ice cover than during the ice free periods. This seasonal variability was
a signiﬁcant factor as the bay was quietest during the ice-break up in July possibly
because it was unsafe for both snowmobiles and boats. The ambient sound levels in
the bay were driven by ice cover through controlling the types of vehicles using the bay
(snowmobiles on ice and boats on open water). Although the observed source levels of
both boats and snowmobiles were similar, in 2015 there were in total ten times more
snowmobile passages than boats. The abundance of snowmobiles passages relative to
boats explains why the bay was louder when there was ice cover. Over the course
of the year, precipitation, wind and ice noise were other important contributors to
the underwater soundscape. Biological noise was not found to be a signiﬁcant sound

86
source.
5.4
Future Directions
The previously undocumented link between the physical environment and the ob-
served variability of detection ranges of passive acoustic tags through multi-path
interference will allow biological studies using this technique to better understand
their results. A natural followup would be to include impacts of the soundscape on
detection ranges.
Although, we were unable to link the soundscape of Cumberland Sound with the
range test results, the Cambridge Bay soundscape indicated at how important this
connection can be. The Cambridge Bay acoustic data coincided with a range test
of the same tags used in Cumberland Sound. Unfortunately, the range test data set
proved to be unusable as it was heavily contaminated by a nearby ship running a
50 kHz sonar from May until September. This corrupted range test does prove an
important point—sounds within an environment, even when they are not at the exact
frequency of the tag used (69 kHz), can profoundly impact the results. This is an
area that should be investigated further.

87
Bibliography
Bacle, J., Carmack, E., and Ingram, R. G. (2002).
Water column structure and
circulation under the North Water during spring transition: April-July 1998. Deep-
Sea Research II, 49:4907–4924.
Barber, D. and Massom, R. (2007). Polynyas: Windows to the world, chapter The
role of sea ice in Arctic and Antarctic polynyas, pages 1–54. Elsevier Oceanographic
Series 74.
Barber, D. G. and Hanesiak, J. M. (2004). Meteorological forcing of sea ice concen-
trations in the southern Beaufort Sea over the period 1979 to 2000. Journal of
Geophysical Research: Oceans, 109(C6).
Bedard, J. M., Vagle, S., Klymak, J. M., Williams, W. J., Curry, B., and Lee, C. M.
(2015).
Outside inﬂuences on the water column of Cumberland Sound, Baﬃn
Island. Journal of Geophysical Research: Oceans, 120:5000–5018.
Bendat, J. S. and Piersol, A. G. (2010). Random Data: Analysis and Measurement
Procedures, 4th Edition. Wiley Series in Probability and Statistics.
Bittencourt, L., Barbosa, M., Secchi, E., Jr, J. L.-B., and Azevedo, A. (2016). Acous-
tic habitat of an oceanic archipelago in the Southwestern Atlantic. Deep Sea Re-
search Part I: Oceanographic Research Papers, 115:103–111.
Bowlin, J., Spiesberger, J., Duda, T., and Freitag, L. (1992). Ocean Acoustical Ray-
Tracing Software RAY. Technical report, Woods Hole Oceanographic Institution
Technical Report.
Butler, J., Stanley, J. A., and IV, M. J. B. (2016). Underwater soundscapes in near-
shore tropical habitats and the eﬀects of environmental degredation and habitat
restoration. Journal of Experimental Marine Biology and Ecology, 479:89–96.

88
Carey, W. M. and Evans, R. B. (2011). Ocean Ambient Nosie: Measurement and
Theory. Springer Science and Business Media.
Carmack, E. (2007). The alpha/beta ocean distinction: A perspective on freshwa-
ter ﬂuxes, convection, nutrients and productivity in high-latitude seas. Deep-Sea
Research II, 54:2578–2598.
Carmack, E. and Wassmann, P. (2006). Food webs and physical-biological coupling
on pan-Arctic shelves: Unifying concepts and comprehensive perspectives. Progress
in Oceanography, 71:446–477.
Clay, C. and Medwin, H. (1977). Acoustic Oceanography: Principles and Applications.
John Wiley & Sons, New York.
Clements, S., Jepsen, D., Karnowski, M., and Schrek, C. (2005). Optimization of an
acoustic telemetry array for detecting transmitter-implanted ﬁsh. North American
Journal of Fisheries Management, 25:429–436.
Cooke, S., Iverson, S., Stokesbury, M., Hinch, S., Fisk, A., VanderZwaag, D., Apos-
tle, R., and Whoriskey, F. (2011). Ocean Tracking Network Canada: a network
approach to addressing critical issues in ﬁsheries and resource management with
implications for ocean governance. Fisheries, 36 (12):583–592.
Cooper, L., Cota, G., Pomeroy, L., Grebmeier, J., and Whitledge, T. (1999). Modiﬁ-
cation of NO, PO, and NO/PO during ﬂow across the Bering and Chukchi shelves:
Implications for use as Arctic water mass tracers. Journal of Geophysical Research,
104(C4):7827–7836.
Crum, L. A., Pumphrey, H. C., Roy, R. A., and Prosperetti, A. (1999). The un-
derwater sounds produced by impacting snowﬂakes. The Journal of the Acoustical
Society of America, 106(4):1765–1770.
Cuny, J., Rhines, P., and Kwok, R. (2005). Davis Strait volume, freshwater and heat
ﬂuxes. Deep-Sea Research I, 52:519–542.
Cuny, J., Rhines, P., Niler, P., and Bacon, S. (2002). Labrador Sea Boundary Currents
and the fate of the Irminger Sea Water. Journal of Physical Oceanography, 32:627–
647.

89
Curry, B., Lee, C., Petrie, B., Moritz, R., and Kwok, R. (2014). Multiyear Volume,
Liquid Freshwater, and Sea Ice Transports through Davis Strait, 2004-10. Journal
of Physical Oceanography, 44:1244–1266.
Dayton, P., Mordida, B., and Bacon, F. (1994). Polar Marine Communities. Integra-
tive and Comparative Biology, 34 (1):90–99.
Dewey, R. (1999). Mooring Design and Dynamics:a Matlab package for designing and
analyzing oceanographic moorings. Marine Models Online, 1:103–157.
Diachok, O. I. (1976). Eﬀects of sea-ice ridges on sound propagation in the Arctic
Ocean. The Journal of the Acoustical Society of America, 59(5):1110–1120.
Dunbar, M. (1958). Physical Oceanographic Results of the ”Calanus” Expedition
in Ungava Bay, Frobisher Bay, Cumberland Sound, Hudson Strait and Northern
Hudson Bay, 1949-1955. Journal of the Fisheries Research Board of Canada, 15
(2):155–201.
Dupont-Prinet, A., Vagner, M., Chabot, D., and Audet, C. (2013). Impact of hypoxia
on the metabolism of Greenland halibut (Reinhardtius hippoglossoides). Canadian
Journal of Fisheries and Aquatic Sciences, 70:461–469.
Dyer, I. (1984).
The song of sea ice and other Arctic Ocean melodies.
In Dyer,
I. and Chryssostomidis, C., editors, Arctic Policy and Technology, pages 11–37.
Hemisphere, New York.
Erbe, C. (2002). An algorithm to predict zones of impact on marine mammals around
underwater industrial noise. Technical report, Canadian Coast Guard, Central and
Arctic Region. pages: 43.
Erbe, C., Verma, A., McCauley, R., Garvilov, A., and Parnum, I. (2015). The marine
soundscape of Perth Canyon. Progress in Oceanography, 137:38–51.
Farmer, D. M. and Vagle, S. (1988). Observations of high-frequency ambient sound
generated by wind. In Kerman, B., editor, Natural Mechanisms of Surface Gener-
ated Noise, page 639. Kluwer.
Finstad, B., Okland, F., Thornstad, E., Bjorn, P., and McKinley, R. (2005). Migration
of hatchery-reared atlantic salmon and wild anadromous brown trout post-smolts
in a norwegian fjord system. Journal of Fish Biology, 66 (1):86–96.

90
Fratantoni, P. and Pickart, R. (2007). The Western North Atlantic Shelfbreak Current
System in Summer. Journal of Physical Oceanography, 37:2509–2533.
Gade, H. G., Lake, R. A., Lewis, E. L., and Walker, E. R. (1974). Oceanography of
an Arctic bay. Deep-Sea Research, 21:547–571.
Garrett, J. K., Blondel, P., Godley, B. J., Pikesley, S. K., Witt, M. J., and Johanning,
L. (2016). Long-term underwater sound measurements in the ship noise indicator
bands 63 Hz and 125 Hz from the port of Falmouth Bay, UK. Marine Pollution
Bulletin, 110:438–448.
Gjelland, K. and Hedger, R. (2013). Environmental inﬂuence on transmitter detection
probability in biotelemetry: developing a general model of acoustic transmission.
Methods in Ecology and Evolution, 4:665–674.
Gladish, C., Holland, D., and Lee, C. (2015).
Oceanic Boundary Conditions for
Jakobshavn Glacier. Part II: Provenance and Sources of Variability of Disko Bay
and Ilulissat Icefjord Waters, 1990-2011. Journal of Physical Oceanography, 45:33–
63.
Greene, C. R. (1995). Ambient noise. In Richardson, W. J., Greene, C. R., Malme,
C. I., and Thomson, D. H., editors, Marine mammals and noise, pages 87–100.
Academic Press, San Diego.
Greene, C. R. and Buck, B. M. (1977). Arctic noise measurement experiments using
Nimbus 6 data buoys. U. S. Navy Journal of Underwater Acoustics, 27:827–838.
Hamilton, J. and Wu, Y. (2013). Canadian Technical Report of Hydrography and
Ocean Science 282: synopsis and trends in the physical environment of Baﬃn
Bay and Davis Strait. Technical report, Ocean and Ecosystem Sciences Division,
Maritimes Region, Fisheries and Oceans Canada.
Hannah, C., Dupont, F., Collins, A., Dunphy, M., and Greenberg, D. (2008). Revi-
sions to a modelling system for tides in the Canadian Arctic Archipelago. Canadian
Technical Report of Hydrography and Ocean Science, 259:1–62.
Haver, S. M., Klinck, H., Nieukirk, S. L., Matsumoto, H., Dziak, R. P., and Miksis-
Olds, J. L. (2017). The not-so-silent world: Measuring Arctic, Equatorial, and
Antarctic soundscapes in the Atlantic Ocean. Deep Sea Research Part I: Oceano-
graphic Research Papers, 122:95–104.

91
Haxel, J. H., Dziak, R. P., and Matsumoto, H. (2013). Observations of shallow water
marine ambient sound: The low frequency underwater soundscape of the central
Oregon coast. The Journal of the Acoustical Society of America, 133(5):2586–2596.
Heupel, M., Reiss, K., Yeiser, B., and Simpfendorfer, C. (2008). Eﬀects of biofouling
on performance of moored data logging acoustic receivers. Limnology and Oceanog-
raphy: Methods, 6:327–335.
Heupel, M., Semmens, J., and Hobday, A. (2006). Automated acoustic tracking of
aquatic animals: scales, design and deployment of listening station arrays. Marine
and Freshwater Research, 57 (1):1–13.
Hofmann, A., Peltzer, E., Walz, P., and Brewer, P. (2011).
Hypoxia by degrees:
Establishing deﬁnitions for a changing ocean. Deep-Sea Research I, 58:1212–1226.
How, J. and de Lestang, S. (2012). Acoustic tracking: issues aﬀecting design, analysis
and interpretation of data from movement studies. Marine & Freshwater Research,
63:312–324.
Hu, X. and Myers, P. (2013). A Lagrangian view of Paciﬁc water inﬂow pathways in
the Arctic Ocean during model spin-up. Ocean Modelling, 71:66–80.
Hussey, N., Kessel, S., Aarestrup, K., Cooke, S., Cowley, P., Fisk, A., Harcourt, R.,
Holland, K., Iverson, S., Kocik, J., Flemming, J. M., and Whoriskey, F. (2015).
Aquatic animal telemetry: A panoramic window into the underwater world. Sci-
ence, 348 (6240).
Huveneers, C., Simpfendorfer, C., Kim, S., Semmens, J., Hobday, A., Pederson, H.,
Stieglitz, T., Vallee, R., Webber, D., Heupel., M., Peddermors, V., and Harcourt,
R. (2016).
The inﬂuence of environmental parameters on the performance and
detection range of acoustic receivers. Ecology and Evolution, 7(7):825–835.
Insley, S. J., Halliday, W. D., and de Jong, T. (2017). Seasonal Patterns in Ocean
Ambient Noise near Sachs Harbour, Northwest Territories. Arctic, 70(3):239–248.
Jones, E., Anderson, L., and Swift, J. (1998). Distribution of Atlantic and Paciﬁc wa-
ters in the upper Arctic Ocean: Implications for circulation. Geophysical Research
Letters, 25(6):765–768.

92
Jones, E., Swift, J., Anderson, L., Lipizer, M., Civitarese, G., Falkner, K., Kattner,
G., and McLaughlin, F. (2003). Tracing Paciﬁc water in the North Atlantic Ocean.
Journal of Geophysical Research, 108 (C4).
Kalnay, E., Kanamitsu, M., Kistler, R., Collings, W., Deaven, D., Gandin, L., Iredell,
M., Saha, S., White, G., Woollen, J., Zhu, Y., Chelliah, M., Ebisuzaki, W., Higgins,
W., Janowiak, J., Mo, K., Ropelewski, C., Wang, J., Leetmaa, A., Reynolds, R.,
Jenne, R., and Joseph, D. (1996). The NCEP/NCAR 40-Year Reanalysis Project.
Bulletin of the American Meteorological Society, pages 427–471.
Kessel, S., Cooke, S., Heupel, M., Hussey, N., Simpfendorfer, C., Vagel, S., and Fisk,
A. (2013). A review of detection range testing in aquatic passive acoustic telemetry
studies. Reviews in Fish Biology and Fisheries. online only.
Kessel, S., Hussey, N., Webber, D., Gruber, S., Young, J., Smale, M., and Fisk,
A. (2015).
Close proximity detection interference with acoustic telemetry: the
importance of considering tag power output in low ambient noise environments.
Animal Biotelemetry, 3:5.
Kinda, G. B., Simard, Y., Gervaise, C., Mars, J. I., d’Heres, M., and Fortier, L.
(2013). Under-ice ambient noise in Eastern Beaufort Sea, Canadian Arctic, and its
relation to environmental forcing. The Journal of the Acoustical Society of America,
134(77):77–87.
Kumlien, L. (1879). Contributions to the Natural History of Arctic America, Made
in Connection with the Howgate Polar Expedition, 1877-78. Bulletin of the United
States National Museum. Washington, D.C.
Lennox, R. J., Aarstrup, K., Cooke, S. J., Cowley, P. D., Deng, Z. D., Fisk, A. T.,
Harcourt, R. G., Heupel, M., Hinch, S. G., Holland, K. N., Hussey, N. E., Iverson,
S. J., Kessel, S. T., Kocik, J. F., Lucas, M. C., Flemming, J. M., Nguyen, V. M.,
Stokesbury, M. J. W., Vagle, S., Vanderzwaag, D. L., Whoriskey, F. G., and Young,
N. (2017). Envisioning the future of aquatic animal tracking: Technology, science,
and application. BioScience, 67:884–896.
Lewis, J. K. and Denner, W. W. (1987). Arctic ambient noise in the Beaufort Sea:
Seasonal space and time scales.
Journal of the Acoustical Society of America,
82(3):988–997.

93
Lillis, A., Eggleston, D. B., and Bohnenstiehl, D. R. (2014). Estuarine soundscapes:
distinct acoustic characteristics of oyster reef compared to soft-bottom habitats.
Marine Ecology, 505:1–17.
Lique, C., Treguier, A., Blanke, B., and Grima, N. (2010). On the origins of water
masses exported along both sides of Greenland: A Lagrangian model analysis.
Journal of Geophysical Research, 115:C05019.
Luczkovich, J. J., Mann, D. A., and Rountree, R. A. (2008). Passive acoustics as a tool
in ﬁsheries science. Transactions of the American Fisheries Society, 137(2):533–541.
Makris, N. C. and Dyer, I. (1991). Environmental correlates of Arctic ice-edge noise.
The Journal of the Acoustical Society of America, 90(6):3288–3298.
McLaughlin, F., Carmak, E., Ingram, R., Williams, W., and Michel, C. (2004). The
Sea, Volume 14, chapter Oceanography of the Northwest Passage, pages 1211–1242.
Harvard University Press.
Medwin, H. and Clay, C. (1998). Fundamentals of Acoustical Oceanography.
Melling, H. (2002). Sea ice of the northern Canadian Arctic Archipelago. Journal of
Geophysical Research, 107 (C11).
Melnychuk, M. and Walters, C. (2010). Estimating detection probabilities of tagged
ﬁsh migrating past ﬁxed receiver stations using only local information. Canadaian
Journal of Fisheries and Aquatic Sciences, 67:641–658.
Merchant, N. D., Pirotta, E., and Thompson, T. R. (2014). Monitoring ship noise to
assess the impact of coastal developments on marine mammals. Marine Pollution
Bulletin, 78(1-2):85–95.
Michel, C., Ingram, R., and Harris, L. (2006).
Variability in oceanographic and
ecological processes in the Canadian Arctic Archipelago. Progress in Oceanography,
71:379–401.
Milne, A. R. (1972). Thermal tension cracking in sea ice: A source of under-ice noise.
Journal of Geophysical Research, 77:2177–2192.
Milne, A. R., Ganton, J. H., and McMillin, D. J. (1967). Ambient noise under sea
ice and further measurements of wind and temperature dependence. The Journal
of the Acoustical Society of America, 41.

94
Myers, P., Donnelly, C., and Ribergaard, M. (2009). Structure and variability of
the West Greenland Current in Summer derived from 6 repeat standard sections.
Progress in Oceanography, 80:93–112.
Myers, P. and Ribergaard, M. (2013). Warming of the Polar Water Layer in Disko
Bay and Potential Impact on Jakobshavn Isbrae. Journal of Physical Oceanography,
43:2629–2640.
Nystuen, J. A. (1986). Rainfall measurements using underwater ambient noise. The
Journal of the Acoustical Society of America, 79:972–982.
Ottera, H. and Skilbrei, O. (2016). Inﬂuence of depth, time and human activity on
detection rate of acoustic tags: a case study on two ﬁsh farms. Journal of Fish
Biology, 88:1229–1235.
Payne, N., Gillanders, B., Webber, D., and Semmens, J. (2010). Interpreting dial
activity patterns from acoustic telemetry: the need for controls. Marine Ecology
Progress Series, 419:295–301.
Peklova, I., Hussey, N., Hedges, K., Treble, M., and Fisk, A. (2012). Depth and
temperature preferences of the deepwater ﬂatﬁsh Greenland halibut Reinhardtius
hippoglossoides in an Arctic marine ecosystem. Marine Ecology Progress Series,
467:193–205.
Pieretti, N., Martire, M. L., Farina, A., and Danovaro, R. (2017). Marine soundscape
as an additional biodiversity monitoring tool: A case study from the Adriatic Sea
(Mediterranean Sea). Ecological Indicators, 83:13–20.
Prinsenberg, S. and Bennett, E. (1989). Vertical Variations of Tidal Currents in Shal-
low Land Fast Ice-Covered Regions. Journal of Physical Oceanography, 19:1268–
1278.
Pritchard, R. S. (1984). Arctic Ocean background noise caused by ridging of sea ice.
The Journal of the Acoustical Society of America, 75(2):419–427.
Rignot, E., Velicogna, I., van der Broke, M., Monaghan, A., and Lenerts, J. (2011).
Acceleration of the contribution of the Greenland and Antarctic ice sheets to sea
level rise. Geophysical R, 38:L05503.

95
Rountree, R. A., Gilmore, R. G., Goudey, C. A., Hawkins, A. D., Luczkovich, J. J.,
and Mann, D. A. (2006). Listening to ﬁsh. Fisheries, 31(9):433–446.
Rudels, B. (2012). Arctic Ocean circulation and variability - advection and external
forcing encounter constraints and local processes. Ocean Science, 8:261–286.
Sanchez-Gendriz, I. and Padovese, L. R. (2016). Underwater soundscape of marine
protected areas in the south Brazilian coast. Marine Pollution Bulletin, 105:65–72.
Scott, W. and Scott, M. (1988). Atlantic Fishes of Canada. Canadian Bulletin of
Fisheries and Aquatic Sciences No. 219.
Simpfendorfer, C., Heupel, M., and Collins, A. (2008). Variation in the performance
of acoustic receivers and its implication for positioning algorithms in a riverine
setting. Canadian Journal of Fisheries and Aquatic Sciences, 65:482–492.
Simpfendorfer, C., Huveneers, C., Steckenreuter, A., Tattersall, K., Hoenner, X.,
Harcourt, R., and Heupel, M. (2015).
Ghosts in the data: false detection in
VEMCO pulse position modulation acoustic telemetry monitoring equipment. An-
imal Biotelemetry, 3:55.
Singh, L., Downey, N., Roberts, M., Webber, D., Smale, M., van den Berg, M.,
Harding, R., Engelbrecht, D., and Blows, B. (2009). Design and calibration of an
acoustic telemetry system subject to upwelling events. African Journal of Marine
Science, 31 (1):355–364.
Staaterman, E., Rice, A. N., Mann, D. A., and Paris, C. B. (2013). Soundscapes from
a Tropical Eastern Paciﬁc reef and a Caribbean Sea reef. Coral Reefs, 32:553.
Steiner, N., Azetsu-Scott, K., Galbraith, P., Hamilton, J., Hedges, K., Hu, X., Janjua,
M., Lambert, N., Larouche, P., Lavoie, D., Loder, J., Melling, H., Merzouk, A.,
Meyers, P., Perrie, W., Peterson, I., Pettipas, R., Scarratt, M., Sou, T., Starr,
M., Tallmann, R., and van der Baaren, A. (2013). Climate Change Assessment in
the Arctic Basin Part 1: Trends and Projections - A Contribution to the Aquatic
Climate Change Adaptation Services Program.
Technical report, Fisheries and
Oceans Canada, Science Branch, Paciﬁc Region.
Straneo, F. and Saucier, F. (2008). The outﬂow from Hudson Strait and its contri-
bution to the Labrador Current. Deep-Sea Research I, 55:926–946.

96
Tang, C., Ross, C., Yao, T., Petrie, B., DeTracey, B., and Dunlap, E. (2004). The
circulation, water masses and sea-ice of Baﬃn Bay.
Progress in Oceanography,
63:183–228.
Tasker, M. L., Amundin, M., Andre, M., Hawkins, A., Lang, W., Merck, T., Scholik-
Schlomer, A., Teilmann, J., Thomsen, F., Werner, S., and Zakharia, M. (2010).
Marine Strategy Framework Directive. Task Group 11 Report - Underwater noise
and other forms of energy. Technical report, Institue for Environment and Sustain-
abiliy.
Thomas, D. N., Fogg, G. E., Convey, P., Fritsen, C. H., Gili, J. M., Gradinger, R.,
Laybourn-Parry, J., Reid, K., and Walton, D. W. H. (2008). The Biology of Polar
Regions. Oxford University Press.
Thorstad, E., Okland, F., Rowsell, D., and McKinley, R. (2000). A system for auto-
mated recording of ﬁsh tagged with coded acoustic transmitters. Fisheries Man-
agement and Ecology, 7 (4):281–294.
Tully, J. P. (1952). Oceanographic Data of the Western Canadian Arctic Region,
1935-37. Journal of Fisheries Research Board of Canada, 8(5):378–382.
Udyawer, V., Chin, A., Knip, D., Simpfendorfer, C., and Heupel, M. (2013). Variable
response of coastal sharks to severe tropical storms; environmental cues and changes
in space use. Marine Ecology Progress Series, 480:171–183.
Urick, R. J. (1983). Principles of Underwater Sound. McGraw-Hill, New York.
Vagle, S., Large, W. G., and Farmer, D. M. (1990). An Evaluation of the WOTAN
Technique of Inferring Oceanic Winds from Underwater Ambient Sound. Journal
of Atmospheric and Oceanic Technology, 7:576–595.
Wenz, G. M. (1962). Acoustic Ambient Noise in the Ocean: Spectra and Sources.
The Journal of the Acoustical Society of America, 34(12):1936–1956.
Williams, R., Wright, A. J., Ashe, E., Blight, L. K., Bruintjes, R., Canessa, R., Clark,
C. W., Cullis-Suzuki, S., Dakin, D. T., Erbe, C., Hammond, P. S., Merchant, N.
D. N. D., O’Hara, P. D., Purser, J., Radford, A. N., Simpson, S. D., Thomas, L.,
and Wale, M. A. (2015). Impact of anthropogenic noise on marine life: Publication
patterns, new discoveries, and future directions in research and management. Ocean
& Coastal Management, 115:17–24.