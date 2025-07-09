# Source: https://doi.org/10.3390/rs15051296
# ID: InterannualVariation

Citation: Zhu, Y.; Zhou, C.; Zhu, D.;
Wang, T.; Zhang, T. Interannual
Variation of Landfast Ice Using
Ascending and Descending
Sentinel-1 Images from 2019 to 2021:
A Case Study of Cambridge Bay.
Remote Sens. 2023, 15, 1296. https://
doi.org/10.3390/rs15051296
Academic Editor: Yi Luo
Received: 17 January 2023
Revised: 13 February 2023
Accepted: 22 February 2023
Published: 26 February 2023
Copyright:
© 2023 by the authors.
Licensee MDPI, Basel, Switzerland.
This article is an open access article
distributed
under
the
terms
and
conditions of the Creative Commons
Attribution (CC BY) license (https://
creativecommons.org/licenses/by/
4.0/).
remote sensing 
Article
Interannual Variation of Landfast Ice Using Ascending and
Descending Sentinel-1 Images from 2019 to 2021: A Case Study
of Cambridge Bay
Yikai Zhu 1,2
, Chunxia Zhou 1,2,*
, Dongyu Zhu 1,2, Tao Wang 1,2
and Tengfei Zhang 3
1
Chinese Antarctic Center of Surveying and Mapping, Wuhan University, Wuhan 430079, China
2
Key Laboratory of Polar Surveying and Mapping, MNR, Wuhan University, Wuhan 430079, China
3
School of Trafﬁc and Transportation Engineering, Changsha University of Science and Technology,
Changsha 410114, China
*
Correspondence: zhoucx@whu.edu.cn
Abstract: Landfast ice has undergone a dramatic decline in recent decades, imposing potential
effects on ice travel for coastal populations, habitats for marine biota, and ice use for industries.
The mapping of landfast ice deformation and the investigation of corresponding causes of changes
are urgent tasks that can provide substantial data to support the maintenance of the stability of the
Arctic ecosystem and the development of human activities on ice. This work aims to investigate the
time-series deformation characteristics of landfast ice at multi-year scales and the corresponding
inﬂuence factors. For the landfast ice deformation monitoring technique, we ﬁrst combined the small
baseline subset approach with ascending and descending Sentinel-1 images to obtain the line-of-sight
deformations for two ﬂight directions, and then we derived the 2D deformation ﬁelds comprising the
vertical and horizontal directions for the corresponding periods by introducing a transform model.
The vertical deformation results were mostly within the interval [−65, 23] cm, while the horizontal
displacement was largely within the range of [−26, 78] cm. Moreover, the magnitude of deformation
observed in 2019 was evidently greater than those in 2020 and 2021. In accordance with the available
data, we speculate that the westerly wind and eastward-ﬂowing ocean currents are the dominant
reasons for the variation in the horizontal direction in Cambridge Bay, while the factors causing
spatial differences in the vertical direction are the sea-level tilt and ice growth. For the interannual
variation, the leading cause is the difference in sea-level tilt. These results can assist in predicting the
future deformation of landfast ice and provide a reference for on-ice activities.
Keywords: landfast ice; 2D deformation; SBAS-InSAR; interannual variation
1. Introduction
As an integral and sensitive component of the global climate system, sea ice can
regulate the exchange of ﬂuxes and radiative transfer between air and sea; it can also alter
sea-surface temperature and salinity, contributing signiﬁcantly to climate and environmen-
tal change in polar regions [1,2]. Landfast ice is a type of sea ice that is attached to a shore,
to the front of an ice shelf, or to the perimeter of a grounded iceberg, and it is a signiﬁcant
component of the local physical and ecological systems [3]. Compared with ﬂoating sea
ice, landfast ice plays a more crucial role in the assessment of a climate system given its
characteristics of a long ice age, greater thickness, and notable seasonal trends [3,4]. Arctic
landfast ice forms a rigid interface between the sea and air in nearshore areas, effectively
buffering coastal erosion due to waves and, thus, inﬂuencing the nearshore sedimentation
and shoreline development [5]. Landfast ice also serves as a key habitat for marine biota
and as a platform for coastal populations to live on [6]. Over the last century, the expansion
of transportation and resource exploitation has triggered increased human activities and
further diversiﬁcation of ice use in the Arctic [7]. As global temperatures have continued to
Remote Sens. 2023, 15, 1296. https://doi.org/10.3390/rs15051296
https://www.mdpi.com/journal/remotesensing

Remote Sens. 2023, 15, 1296
2 of 25
rise since the 1970s, climatic conditions in high-altitude areas of the Arctic have dramatically
changed and sea ice has been rapidly declining [8], exposing ice users to unknown risks,
as well as potential hazards [9]. The continued warming of the Arctic and the accelerated
melting of sea ice—particularly the remarkable reduction in the extent of landfast ice in
coastal areas—have led to increased ship trafﬁc and resource exploration [10]. Therefore,
determining landfast ice deformation is particularly important for local populations and
organisms to anticipate potential hazard events and provide effective early warning in real
time. Awareness is growing that sea ice conditions for Arctic marine operations will be a
challenge that will require considerable monitoring and advanced methods in the future.
This improvement must be implemented on a large scale and a regional scale to assess
environmental hazards and develop effective and timely preventive measures.
The accurate determination of the large-scale and long-term deformation of landfast ice
while ensuring the immediate validity and feasibility of a method has been a challenge [11].
Observing sea ice conditions in situ is difﬁcult due to the speciﬁcity and complexity of
the terrain. Satellite remote sensing is an essential tool for detecting sea ice parameters.
Optical remote sensing is always effective in investigating sea ice properties, such as the
extent [12] and type classiﬁcation [13], with the characteristics of large spatial coverage, high
spatio-temporal resolution, and a huge accessible database. However, optical methods are
ineffective in making timely measurements due to weather limitations, such as clouds and
fog. By the early 1990s, synthetic aperture radar (SAR) was introduced to create a superior
dataset for sea ice studies, given its advantages of performing in all weather, working on
all days, higher resolution, and wider monitoring coverage [5]. Intensity information from
SAR has been widely applied for sea ice classiﬁcation [14] and the analysis of motion [15],
thickness [16], concentration [17], roughness [18] and snow-cover extent [19–21]. However,
SAR backscatter does not typically provide deformation information about landfast ice
or transiently stable ﬂoating ice, as the internal motion of landfast ice is too small to be
identiﬁed via change detection. The advent of the SAR interferometry (InSAR) technology
has opened up the possibility of using SAR data to monitor landfast ice deformation by
extracting the phase difference between two SAR images with similar imaging geometries.
In this manner, topography can be retrieved if acquisitions are separated in space (nonzero
perpendicular baseline), or surface movement along the line-of-sight (LOS) direction can be
measured if it is separated in time (nonzero temporal baseline) [22,23]. For example, Li et al.
ﬁrst introduced InSAR into the detection of sea ice deformation [24]. Wang et al. combined
ascending- and descending-pass images to obtain the vertical and horizontal deformations
of landfast ice for a single period in the Baltic Sea [25]. The preceding studies only retrieved
the LOS or a 2D deformation ﬁeld for a single period. This information is evidently
insufﬁcient for characterizing the deformation of landfast ice. With the launches of the
Sentinel-1A satellite in 2014 and the Sentinel-1B satellite in 2016, ascending and descending
datasets that cover the same area can be utilized to derive a 2D deformation ﬁeld of landfast
ice over a long period. Although ascending and descending SAR data cannot be combined
for interferometry, they can provide InSAR observations with different imaging geometries,
and 3D surface deformation can be estimated by building a multi-source heterogeneous
InSAR observation model [26,27].
The coherence of ice as a monitored object is highly susceptible to external factors,
such as temperature, precipitation, and wind. The small baseline subset (SBAS) InSAR
technique ensures the high coherence and reliability of the inversion results by selecting
interferometric combinations with small spatio-temporal baselines that are as far as pos-
sible [26,28]. SBAS-InSAR is a speciﬁc type of multi-temporal InSAR that utilizes a small
subset of images to create highly accurate results [29]. This method is ideal for areas where
the availability of satellite data is limited or for monitoring large areas that would otherwise
be impractical with traditional InSAR. One of the key beneﬁts of SBAS-InSAR is its ability
to detect even small changes in the Earth’s surface over time. This makes it a valuable
tool for monitoring the effects of natural disasters, such as earthquakes [30] and volcanic
eruptions [31], as well as human-made changes, such as urban development and subsi-

Remote Sens. 2023, 15, 1296
3 of 25
dence [32]. Additionally, SBAS-InSAR can be used to monitor ground deformation and
subsidence in areas where oil and gas extraction is taking place [33], which helps to ensure
the safety of nearby communities and infrastructure. Another advantage of SBAS-InSAR
is that it is a non-invasive technique that does not require any physical access to the area
being monitored. This makes it ideal for monitoring remote or difﬁcult-to-reach areas, such
as mountainous regions. Based on this, it has a great potential to be applied in monitoring
natural disasters of the cryosphere, such as ice avalanches [34] and sea ice motion [35].
The current work uses ascending and descending Sentinel-1 images and the SBAS
approach to derive LOS deformation results for both modes from January to April in
2019–2021. Then, the vertical and horizontal time-series deformations can be decomposed
on the basis of a transform model. Finally, the temporal and spatial deformation character-
istics of the test area and the factors that affect the deformation changes are analyzed in
relation to the experimental results.
2. Methods
2.1. SBAS-InSAR Data Processing
SBAS-InSAR is a powerful remote sensing technique that is used to monitor changes
in the Earth’s surface over time [36]. This technology combines the use of SAR images with
the principle of interferometry to produce highly accurate and detailed maps of the Earth’s
surface [37]. The procedure of SBAS-InSAR involves several steps, as shown in the pink
frame of Figure 1.
(1) Data acquisition: The ﬁrst step in SBAS-InSAR processing is to acquire the necessary
SAR images. This typically involves using a SAR satellite to acquire multiple images of the
same area at different times. The images should be acquired from similar viewing angles to
ensure the highest accuracy.
(2) Image preprocessing: Once the SAR images have been acquired, they must be
preprocessed to remove any noise or errors in the data. This may include correction for
atmospheric effects, radiometric calibration, and speckle ﬁltering [38].
(3) SBAS network: The next step is to select a small subset of the SAR images to use for
the SBAS-InSAR analysis. This subset should be chosen based on the spatial and temporal
distribution of the images and the desired accuracy of the results.
(4) Interferogram stacks: After the small baseline subset has been selected, an interfer-
ogram can be generated by applying interferometric techniques to the subset of images.
The interferogram provides information about the relative height differences between the
surfaces in the two images and is used to generate the ﬁnal SBAS-InSAR results.
(5) Phase unwrapping: The next step is to unwrap the phase information in the
interferogram to remove any phase ambiguities. This is typically done by using algorithms
such as the least-squares method or the multichannel algorithm [39].
(6) Time-series deformation inversion: A singular value decomposition (SVD) is uti-
lized here to estimate the minimal norm least-squares solution of the average deformation
rate of each period [40,41]; meanwhile, the times-series deformation can be calculated as
well [42].
2.2. Calculation of Horizontal and Vertical Deformations
The time-series deformation results of ascending and descending orbits were derived
via the SBAS method. In this work, the time-series deformation results were used in
combination with the classical function model to monitor the 3D deformation of landfast
ice in the study area.
The classical function model was established on the basis of the geometric relationship
between the InSAR observations and 3D deformations. Figure 2 shows a schematic dia-
gram of 3D deformations from different perspectives. In accordance with Figure 2a,b, the
deformation vector along the LOS direction is located on a 2D plane composed of a ground
direction (projection of the LOS direction on the horizontal plane) and a vertical direction.

Remote Sens. 2023, 15, 1296
4 of 25
The ground range and vertical direction are perpendicular to each other; thus, the following
expression can be obtained according to the geometric relationship in Figure 2a [43]:
dA
LOS = du · cos θA
inc + dh · sin θA
inc
(1)
where du and dh deﬁne, respectively, the vertical and ground displacement vectors; θA
inc
represents the pixel-based incidence angle of the radar pulse signal.
Figure 1. The workﬂow of the methodology is composed of two parts: SBAS processing and 2D
deformation resolution.
The ground displacement vector is composed of the eastern and northern components,
as illustrated in Figure 2b [30,44].
dh = dc ·
h
−sin

αA
AZI −3π/2
i
+ dn ·
h
−cos

αA
AZI −3π/2
i
(2)
where de and dn represent, respectively, the east–west and north–south displacement
vectors; αA
AZI is the orbit azimuth angle (positive clockwise from the north), and αA
AZI −3π/2
actually deﬁnes the angle between the ground range direction and the north. By substituting
Equation (2) into Equation (1), the relationship between the LOS deformation and the 3D
surface deformation vector can be established in the case of the ascending mode.
dA
ios = −dc · sin θA
inc · sin

αA
AZI −3π/2

−dn · sin θA
inc · cos

αA
AZI −3π/2

+ du · cos θA
inc
(3)
Similarly, the relationship between the LOS deformation and 3D surface deformation
can be constructed in the descending mode as follows [45]:
dD
LOS = −dc · sin θD
inc · sin

αD
AZl −3π/2

−dn · sin θD
inc · cos

αD
AZI −3π/2

+ du · cos θD
inc
(4)

Remote Sens. 2023, 15, 1296
5 of 25
Sentinel-1 is a near-polar orbit satellite with a low sensitivity in the north–south
direction. Therefore, performing high-precision detection of movement in this direction is
difﬁcult. We assume that the north–south deformation of landfast ice in the study area is
zero. Then, 2D deformations comprising the east–west and vertical deformations can be
decomposed from the combination of ascending and descending results of the same epoch.
This issue can be resolved by combining Equations (3) and (4).
 du
de

=
 cos θA
inc
−sin θA
inc sin
 αA
AZI −3π/2

cos θD
inc
−sin θD
inc sin
 αD
AZI −3π/2

−1 dA
LOS
dD
LOS

(5)
Figure 2. Geometric relationship of the LOS direction from (a) the horizontal perspective and (b) the
vertical perspective in the Sentinel-1A ascending orbit mode, as well as (c) the imaging geometry of
ascending and descending images.
3. Study Area and Datasets
3.1. Cambridge Bay
The study site was located in the southern part of Cambridge Bay, as shown in Figure 3.
Cambridge Bay is a hamlet located on Victoria Island in the Kitikmeot Region in Nunavut,
Canada. Given its unique geographical situation, i.e., it is located on the western end of
Queen Maud Gulf, where it narrows into Dease Strait, Cambridge Bay is a transportation
and administrative hub for the Kitikmeot Region and the largest stop for passenger and
polar research vessels traversing the Arctic Ocean’s Northwest Passage [46]. Cambridge
Bay has a polar climate, that is, no month has an average temperature of 10 ◦C or higher. In
accordance with the regional sea ice charts available from the Canadian Ice Service Data
Archive (CISDA), landfast ice dominates the ice type, with various thickness values at
different dates during the winter [47].

Remote Sens. 2023, 15, 1296
6 of 25
Figure 3. Study area: (a) Location of Cambridge Bay, with the blue and red rectangles representing,
respectively, the coverage of ascending and descending images; the black rectangle deﬁnes the
zoomed-out view of (b); the yellow circles and blue circle represent the oceanographic stations
and ice proﬁle. (b) Speciﬁc location of feature and reference points of phase unwrapping, with the
Sentinel-2 images being the base map. (c) Site picture of Cambridge Bay where reference point R is
located.
3.2. Sentinel-1 Images and Reference Data
The Sentinel-1 mission comprises a constellation of two polar-orbiting satellites
(Sentinel-1A and Sentinel-1B, which share the same orbital plane) that operate day and
night to perform C-band SAR imaging, thus enabling them to offer reliable and repeated
wide-area monitoring regardless of the weather [48]. To investigate the interannual de-
formation characteristics of landfast ice in the area of interest, the three-year (2019–2021)
Sentinel-1 single-look complex (SLC) products over Cambridge Bay were used to retrieve
the 2D deformation results (the speciﬁc characteristics are listed in Table 1). Considering
the coherence level of the observed objects, SAR images from January to April in each year
were selected for the experiments to avoid the incoherence caused by ice melting. The
interferometric processing was accomplished with the GAMMA software, which included
interferometry and phase unwrapping. In the process of SBAS-InSAR processing, the
master image of each subset was connected to the two slave images whose acquisition dates
were the closest to that of the master (Figure A1) to minimize the probability of temporal
decoherence and obtain ideal interferometric and unwrapping results. The coherence
threshold of 0.3 was adopted to ensure relatively reliable high-quality point selection. The
multi-look ratio was set to 10:2 in this work. In addition, a Goldstein ﬁlter was used to
suppress the noise phase, and the commonly used minimum cost ﬂow algorithm was
applied for phase unwrapping. The reference point R of phase unwrapping was located in
the residential area of Cambridge Bay, which was regarded as stable (Figure 3).
The temperature, tide, and wind of two weather stations (Austin Bay and Cambridge
Bay, the locations of which are shown in Figure 3) were collected from the ofﬁcial website of
the Government of Canada to understand the ice conditions and interpret the deformation
characteristics in Cambridge Bay. Moreover, the ice draft data available from the Ocean
Networks Canada Data Archive were downloaded and are displayed in Figure 4.

Remote Sens. 2023, 15, 1296
7 of 25
Table 1. Characteristics of the Sentinel-1 images used in this work.
Flight Direction
Beam Mode
Polarization
Incidence Angles (°)
Repeat Cycle (Days)
Number of Scenes Acquired
Ascending
IW
VV
38.98
12
28
Descending
IW
HH
39.15
12
29
Figure 4. Ice draft changes from an ice proﬁler near Cambridge Bay. The part with a light-blue
background is the period that covers the acquisition dates of the SAR images used in this work.
4. Results
4.1. Average Coherence and Interferograms
Coherence in InSAR is deﬁned as the cross-correlation coefﬁcient in the registration
of two SLCs [49]. It is a widely used byproduct of InSAR [50,51]. It is also a signiﬁcant
indicator that is generally used to evaluate the quality of the interferometric phase. A higher
coherence can effectively alleviate the problem of phase discontinuity, which introduces
errors in the follow-up steps, yielding a more reliable displacement result. In the current
work, the average coherence from C-band Sentinel-1 InSAR pairs was counted and is
displayed in Figure A2 for the period of January–April 2019–2021. The majority of average
coherence values were within the range of [0.3, 0.4]. The coherence level of landfast ice
in the present work was evidently lower than those of traditional research objects, such
as roads, bridges, mining areas, and earthquakes [52,53]. The factors for decorrelation
could mostly be attributed to the physical properties of the ice, which was susceptible to
temperature, wind, and precipitation. In addition, the ice growth exhibited the potential
to decrease coherence by increasing freeboard, and the internal random motion (e.g., the
dynamics of brine inclusions) of ice and snow could also lower it [22].
From Equation (5), we acquired two interferogram subsets (ascending and descending)
for each year during the period of 2019–2021. Given the form of the fringes, they were
unlikely to be caused solely by the displacement on the surface snow layer or ice growth.
Instead, the inducer was likely to be a mechanical metamorphosis. We chose a representa-
tive selection of ﬁltered interferograms from each subset for presentation (Figure A3). Each
color-cycle change indicates that the deformation change along the LOS direction is half of
a wavelength in length. Dense interferometric fringes correspond to a large phase gradient.
Rather, a sparse fringe distribution indicates that the region remained relatively stable. As
a general rule, horizontal movement dominated the surface deformation when the fringes
exhibit an evident parallel trend, while circular fringes indicate that vertical deformation
played a major role [54]. From the spatial distribution shown in Figure A3, sparse suborbic-
ular fringes were distributed near Cambridge Bay, while the quantity of fringes gradually
increased with increasing distance from land, and the fringes were arranged in parallel.

Remote Sens. 2023, 15, 1296
8 of 25
This ﬁnding suggests that landfast ice closer to the shore was more stable under the effect
of a buttressing force. In terms of time-series variation, high-density interferometric fringes
were spread across the study area in January and February. With the passing of time, the
quantity and shape of fringes changed markedly from March to April, with mostly sparse
circular fringes, indicating that the deformation in this area was relatively signiﬁcant from
January to February, and the landfast ice gradually became stable from March to April,
with enhanced interference immunity. Some fringe discontinuities could be observed in
these interferograms; these were largely due to the inﬂuence of ice ridges on the ice roads
caused by anthropogenic activities during winter. In addition, four feature points ( P1, P2,
P3, and P4) were marked for a better exploration of the deformation evolution pattern in
this region. The concrete positions of these points are shown in Figure 3. P1 is located
on land, with the primary use of checking the reliability of the experimental results. P2
is close to Cambridge Bay, where interferometric fringes are sparse and the magnitude of
movement is relatively small. P3 is located near the ice ridge. It can be used to investigate
the deformation characteristics of landfast ice with support from external forces. P4 is
located on smooth landfast ice without the interference of visible factors. Numerous dense
fringes are distributed near P4, indicating the existence of a large deformation.
4.2. Ascending and Descending LOS Deformations
As described in Section 2.1, the SBAS method was used to derive the time-series
deformation from 2019 to 2011 on the basis of ascending and descending Sentinel-1 images.
All InSAR measurements were calibrated as changes relative to a reference point, which is
labeled with R in Figure 3. The corresponding LOS deformation results from the ascending
and descending datasets are displayed in Figures A4–A9. A positive or negative sign
represents movement closer to or away from the SAR sensor. In terms of the overall spatial
distribution, the results for the ascending and descending modes reﬂected nearly opposite
deformation trends, with a maximum accumulated movement of −52 cm for ascending
images and 55 cm for descending images. The different geometries and polarizations (HH
was used in the ascending mode, while VV was used in the descending mode) adopted in
the deformation monitoring contributed to the discrepancies in the results of the ascending
and descending images. The same deformation may exhibit the opposite direction to that
of a deformation monitored under the opposite geometry. The time-series deformation
results of the four feature points located in the different areas mentioned in Section 4.1 were
extracted to further reveal the generated deformation characteristics, as shown in Figure 5.
P1 and P2 remained relatively stable in terms of temporal variation, which was closely
related to their geographical locations. P1 was located on land, and point P2 was close
to Cambridge Bay. By contrast, the remaining two feature points had signiﬁcantly larger
deformation magnitudes and exhibited opposite deformation directions for the different
datasets. P3 experienced a downward displacement along the LOS direction at a total of
−4.7 cm, −7 cm, and −9.6 cm from 2019 to 2021 with ascending images, and it presented
the opposite trend at 21 cm, 15 cm, and 20 cm from 2019 to 2021 under the descending
geometry. The deformation trend of P4 was similar to that of P3, and they only differed in
magnitude.

Remote Sens. 2023, 15, 1296
9 of 25
Figure 5. Cumulative deformation from different directions of each feature point: (a) vertical
deformation in 2019; (b) horizontal deformation in 2019; (c) ascending LOS deformation in 2019;
(d) descending LOS deformation in 2019; (e) vertical deformation in 2020; (f) horizontal deformation
in 2020; (g) ascending LOS deformation in 2020; (h) descending LOS deformation in 2020,; (i) vertical
deformation in 2021; (j) horizontal deformation in 2021; (k) ascending LOS deformation in 2021;
(l) descending LOS deformation in 2021.

Remote Sens. 2023, 15, 1296
10 of 25
4.3. Horizontal and Vertical Deformations
The temporal coverage of the Sentinel-1 ascending images was not completely con-
sistent with that of the descending images. The 2D time-series deformation results could
only cover overlapping periods. The horizontal time-series displacement maps from 2019
to 2021 are displayed in Figures 6–8, and the vertical deformation results are presented
in Figures 9–11. A positive or negative sign indicates that the movement was uplifted
(or moved to the east) or sunk (or moved to the west). A vertical deformation within
the range of [−68, 23] cm and a horizontal deformation within the range of [−26, 78] cm
were derived in 2019. Meanwhile, the magnitude of vertical deformation in 2020 and 2021
was apparently lower, i.e., within the range of [−45, 12] cm. In summary, the movement
of the coastal zone tended towards zero in both directions, and it was evidently smaller
than that of the channel. The results showed a predominant subsidence on the western
side, with an overall tendency for landfast ice to migrate eastwards. In terms of the time
sequence, the period with a large deformation rate occurred mostly from early January to
early March. Thereafter, the deformation rate in the study area signiﬁcantly decreased. As
for the horizontal deformation, the landfast ice retained similar variation characteristics
from 2019 to 2021, within the range of [−26, 78] cm. Most areas showed a trend of eastward
movement, especially in the western ﬂank.
The time-series deformation results of the feature points mentioned above were ex-
tracted and are displayed in the ﬁrst and second columns of Figure 5 and Table 2. In
2019, P1, P2, P3, and P4 experienced settlement at 0.1 cm, 0.6 cm, 12.8 cm, and 34.9 cm,
respectively. P1, P2, P3, and P4 experienced eastward movement at 0.1 cm, 0.5 cm, 15.8 cm,
and 35.3 cm, respectively. Similarly, the magnitude of vertical deformation in 2019 was
greater than those in the other two years. In 2020, the cumulative maximum deformations
of P1, P2, P3, and P4 were 0.1 cm, −0.3 cm, −4.1 cm, and −14.8 cm, respectively, in the
vertical direction and 0.1 cm, 0.4 cm, 17.4 cm, and 39.1 cm, respectively, in the horizontal
direction. In 2021, the cumulative maximum deformations of P1, P2, P3, and P4 were 0 cm,
−0.3 cm, −7.6 cm, and −11.9 cm, respectively, in the vertical direction and 0.03 cm, 0.4 cm,
24.7 cm, and 37.5 cm, respectively, in the horizontal direction.
Table 2.
Accumulated deformation along the vertical and horizontal directions from 2019 to
2021 (in cm).
Feature
Points
Years (Movement Direction)
2019
(Vertical)
2019
(Horizontal)
2020
(Vertical)
2020
(Horizontal)
2021
(Vertical)
2021
(Horizontal)
P1
−0.1
0.1
0.1
0.1
0
0
P2
−0.6
0.5
−0.3
0.4
−0.3
0.4
P3
−12.8
15.8
−4.1
17.4
−7.6
24.7
P4
−34.9
35.3
−14.8
39.1
−11.9
37.5
The factors that affected the deformation characteristics will be discussed in Section 5.
In addition, SAR images obtained at different times were slightly different under different
SAR conditions, such as the atmospheric conditions and polarization [55]. This phe-
nomenon led to changes in intensity, coherence, and measurement in the InSAR analysis,
and these are not evaluated in the current work.

Remote Sens. 2023, 15, 1296
11 of 25
Figure 6. Retrieved time-series horizontal deformation from January 2019 to May 2019: the top-right
number of subﬁgures (a–i) represents the corresponding SAR images acquisition dates.
Figure 7. Retrieved time-series horizontal deformation from January 2020 to April 2020: the top-right
number of subﬁgures (a–f) represents the corresponding SAR images acquisition dates.

Remote Sens. 2023, 15, 1296
12 of 25
Figure 8. Retrieved time-series horizontal deformation from January 2021 to April 2021: the top-right
number of subﬁgures (a–j) represents the corresponding SAR images acquisition dates.
Figure 9. Retrieved time-series vertical deformation from January 2019 to May 2019: the top-right
number of subﬁgures (a–i) represents the corresponding SAR images acquisition dates.

Remote Sens. 2023, 15, 1296
13 of 25
Figure 10. Retrieved time-series vertical deformation from January 2020 to April 2020: the top-right
number of subﬁgures (a–f) represents the corresponding SAR images acquisition dates.
Figure 11. Retrieved time-series vertical deformation from January 2021 to April 2021: the top-right
number of subﬁgures (a–j) represents the corresponding SAR images acquisition dates.
4.4. Validation
To compensate for the unavailability of external validation data over Cambridge Bay,
the residual phase of each interferogram was calculated and selected as the accuracy index.
In addition, we also compared the results of this work with those of similar published
studies to verify the reliability of the experimental results.
The residual phase can be estimated by subtracting the low-pass deformation phase
from the unwrapped phase, and this has the ability to evaluate the accuracy of deformation
modeling in InSAR analysis [56]. A smaller residual phase indicates a higher accuracy of

Remote Sens. 2023, 15, 1296
14 of 25
deformation inversion. The average residual phase for each subset of SBAS processing is
provided in Table 3. Its value was less than 1 rad in any ﬂight direction and acquisition
year, demonstrating that the conventional linear model used in the SBAS method was
sufﬁciently accurate for deformation estimation in the current work.
Table 3. Average residual phases of different ﬂight directions from 2019 to 2021 (in rad).
Flight
Direction
Year
2019
2020
2021
Ascending
0.85
0.67
0.75
Descending
0.68
0.71
0.81
To our knowledge, few cases have used InSAR techniques—particularly multi-temporal
InSAR—to monitor landfast ice deformation. Marbouti et al. ﬁrst explored Sentinel-1 im-
ages with the DInSAR technology to retrieve deformations in 2015; a deformation of
landfast ice on the order of 40 cm was observed in the Baltic Sea [57]. Only a single period
of LOS deformation was obtained here due to the paucity of data, thus failing to recover
the true 3D deformation ﬁeld. Wang et al. combined ascending and descending data to
retrieve the vertical and horizontal deformations of landfast ice in the Baltic Sea [25]. They
extended the dimensionality of InSAR technology to landfast ice deformation monitoring.
For long-term monitoring, Chen et al. utilized the multi-dimensional SBAS method to
process time-series observations for retrieving 2D deformations that comprised the vertical
and horizontal directions in Cambridge Bay [58]. At present, only Chen et al. has applied
multi-temporal InSAR to the monitoring of landfast ice deformation in Cambridge Bay.
However, they only investigated deformation characteristics for 1 year and did not sys-
tematically address the potential causes of deformation. In the current work, ascending
and descending datasets were used to detect the time-series 2D deformation for 3 years
(2019–2021), and the possible causes of deformation are summarized and discussed in a
later section. Given the lack of external in situ measurements, we compared the results
retrieved for the same geographical location with those of previous studies to verify the
accuracy and improvements of the time-series 2D deformation in this work. The deforma-
tion results for the same acquisition period (2019) are listed in Table 4. A small deformation
difference indicates that the two results maintained a high level of agreement, thus proving
the reliability and authenticity of the experimental results in the current work.
Table 4. Comparison of deformation results (in cm).
Deformation
Results
LOS
2D
Ascending
Descending
Vertical
Horizontal
Study of Chen et al.
−13.6
16.7
9.4
21.7
Results in the current work
−14.6
19.0
5.7
23.2
Deformation difference
1.0
−2.3
2.7
−1.5
5. Discussion
5.1. Suggested Reasons for Deformation
In this work, 2D deformation results that included the east–west and vertical displace-
ments of landfast ice in Cambridge Bay from January to April in 2019–2021 were obtained
by combining ascending and descending images. The suggested reasons for the deforma-
tions are described from two perspectives. The factors that affect horizontal deformations
include ocean currents, winds, and thermal expansion [25]. Meanwhile, the factors that
affect vertical deformation include temperature, ice growth, and sea-level tilt [58], which
are further described below.

Remote Sens. 2023, 15, 1296
15 of 25
The horizontal deformation of the landfast ice presented an eastward migration trend,
except for landfast ice around the coastal area, which remained relatively stable. The
cold ocean current from Beaufort Sea ﬂows through Amundsen Gulf, Coronation Gulf,
and Cambridge Bay from west to east [59], indicating the west-to-east current direction
in this area. Thus, the cold ocean current possesses the ability to promote the eastward
movement of landfast ice near Cambridge Bay in the horizontal direction. For the winds,
their direction and magnitude exerted a certain degree of inﬂuence on the horizontal
deformation. Figure 12 shows the wind information of the corresponding research period
(2019–2021), including the speed and direction. Overall, the study area was dominated by
westerly winds. The continuous winds from the west caused ice ﬂoes to drift eastward and
compress the western boundary of the landfast ice, resulting in an eastward movement.
Simultaneous dilatation was observed along the western boundary, which typically occurs
under such forcing conditions [60]. In particular, the largest horizontal displacement rate for
P3 occurred between 4 January 2019 and 5 March 2019, with an accumulative deformation
of 14 cm. As shown in Figure 12, the wind direction remained unchanged, while the speed
rose from 17 km/h to 27 km/h, indicating an enhanced wind force. The combined actions
of the wind speed and direction caused the most severe horizontal movement in this period.
Ice draft data collected from an ice proﬁler are shown in Figure 4. Ice growth began
in late October every year and consistently maintained an upward trend, exhibiting some
ﬂuctuations before reaching the maximum value of approximately 1.8 m in the middle
of or in late May. In general, a 9:1 proportional relationship existed between the ice draft
and freeboard [61], that is, the thickness of the freeboard would increase by approximately
20 cm when the increment in the ice draft reaches 1.8 m. This assumption was inconsistent
with the deformation magnitude observed in the current work. Therefore, except for the
growth of ice, variations in sea level should be taken into account with regard to the
vertical movement of landfast ice. Absolute sea-level changes cannot be reﬂected in an
interferogram, as landfast ice is aﬂoat, but sea-level tilt affects the distribution of vertical
movement. Fringes can be affected by sea-level variation in accordance with ∆R = ϕ/2k ,
where ϕ deﬁnes the interferometric phase, and k = 2π/λ represents the wave number [62].
Horizontal movements could be estimated through the placement of oceanographic stations.
Two oceanographic stations, Austin Bay and Cambridge Bay, could provide the required
tidal data with a sampling frequency of 1 h. The corresponding spatial distribution of
vertical deformation could be derived by measuring the differences in sea-level changes
between the two meteorological stations during the same period. For example, the sea
level in Austin fell from 18 cm to 8 cm from 31 December 2018 to 22 April 2019, whereas in
Cambridge Bay, a reduction from 46 cm to 45 cm was observed. Therefore, the maximum
relative change in the water level was 9 cm. According to the statistical results presented in
Figure 13, the relative sea-level variation amplitude in the Austin area was higher than that
in Cambridge Bay during most periods, leading to a phenomenon in which the settlement
magnitude in the west was greater than that in the east. This phenomenon was consistent
with the spatial distribution of the vertical deformation.

Remote Sens. 2023, 15, 1296
16 of 25
Figure 12. Collected wind data, including direction and speed, in (a) 2019, (b) 2020, and (c) 2021.
Figure 13. Sea-level tilt between Cambridge Bay and Austin Bay: (a) 2019, (b) 2020, and (c) 2021.
5.2. Possible Causes of Interannual Variation
In accordance with the interannual experimental results, the variability of landfast
ice in the study area tended to be stable. The magnitude of deformation in 2019 was
signiﬁcantly higher than those of the other two years. By collating and analyzing the
external data, we determined that the three-year average temperatures during the period
covered by the SAR images were −27.4 °C, −28.4 °C, and −27.6 °C, with slight variations.
Meanwhile, the maximum temperature was considerably below 0 °C. The possibility
that melting caused the severe deformation of the ice is inexistent. According to the

Remote Sens. 2023, 15, 1296
17 of 25
previous analysis, ice growth and sea-level tilt are the most likely factors that affect vertical
movement in this area. The growth rate and maximum value of the ice draft in these 3 years
were nearly equal, as shown in Figure 4. The sea-level tilt for each acquisition date in 2019
was evidently larger than those for the other two years. For example, the east–west tidal
tilt from January to March 2019 was 0.5 cm, while those for the same periods in 2020 and
2021 were only 0.1 cm and 0.2 cm, respectively. We speculate that this phenomenon may
have been one of the most important reasons for the vertical deformation of landfast ice in
2019. During the three studied years, the wind was dominated by westerlies, with minimal
variation in wind speed. No extreme weather or ocean current events were reported in the
study area in 2019–2021. Consequently, the deformation characteristics in the horizontal
direction remained relatively immobile. In addition, the random internal motion of landfast
ice would also have an inﬂuence on the deformation results. This inﬂuence was not
considered in this work.
6. Conclusions
In the current work, the time-series 2D deformation that comprised the horizontal
and vertical directions over the landfast ice area in Cambridge Bay was investigated by
using ascending and descending datasets from Sentinel-1A and the multi-dimensional
SBAS approach. This work is the ﬁrst to utilize the multi-dimensional SBAS approach to
explore the interannual variations (2019–2021) in landfast ice. We ﬁrst obtained the LOS
deformation of landfast ice from January to April in 2019–2021, and we discovered that the
experimental results obtained from the ascending and descending datasets tended to exhibit
opposite deformation directions due to differences in their imaging geometries. Then, the
time-series 2D deformation could be decomposed from the ascending and descending
LOS deformation by introducing a transform model. The derived results revealed that
the vertical deformation in the study area was mostly within the interval of [−65, 23] cm,
while the horizontal deformation was largely within the range of [−26, 78] cm. The
maximum annual deformation rate of landfast ice occurred from early January to the end
of March, while the movement remains relatively ﬂat from April to May. Furthermore,
the comparison of the results for the three years indicated that the magnitude of the time-
series deformation results obtained in 2019 was signiﬁcantly larger than those in the other
two years, particularly in the vertical direction.
With the collected external data, including tides, temperature, ice draft, and wind, the
factors that affected deformation were classiﬁed into two categories in accordance with
the deformation directions. The ﬁrst category included the inﬂuencing factors associated
with vertical deformation, such as ice growth, temperature, and sea-level tilt. The second
category comprised the inducers acting on the horizontal deformation, such as ocean
currents and winds. We concluded that wind and sea-level tilt dominated the variability in
the deformation in Cambridge Bay, with the largest contributor to the magnitude anomalies
of deformation in 2019 being the differences in sea-level tilt changes.
Overall, this work utilized the SBAS-InSAR method to determine interannual landfast
ice deformation, which is particularly important for local populations and organisms in
anticipating potential hazard events and providing effective early warnings in real time. In
addition, this proved that the multi-temporal InSAR technology possesses the capacity of
being introduced into surface deformation monitoring in the cryosphere. This work still has
some weaknesses. Firstly, deriving the time-series deformation for a period of more than 5
or 10 years is difﬁcult due to the limited data that are currently available. This issue may
be addressed in the future with the launches of more SAR satellites, such as Sentinel-1C,
ALOS-3, and Tandem-L. Secondly, the internal variability, such as the dynamics of brine
inclusions and ice rheology, was not considered when analyzing the inﬂuencing factors.
Therefore, we will be involved in using longer time series and multi-source data to conduct
research and analyze the deformation patterns from a multidisciplinary perspective in the
future.

Remote Sens. 2023, 15, 1296
18 of 25
Author Contributions: Conceptualization, Y.Z.; methodology, Y.Z.; software, Y.Z. and T.Z.; validation,
Y.Z.; formal analysis, Y.Z. and C.Z.; investigation, Y.Z. and C.Z.; resources, Y.Z. and T.Z.; data curation,
Y.Z.; writing—original draft preparation, Y.Z.; writing—review and editing, Y.Z., C.Z., D.Z. and
T.W.; visualization, Y.Z.; supervision, C.Z.; project administration, C.Z.; funding acquisition, C.Z. All
authors have read and agreed to the published version of the manuscript.
Funding: This research was funded by the National Key Research and Development Program of
China (2021YFC2803302), the National Natural Science Foundation of China (42171133, 41941010),
and the Funds for the Distinguished Young Scientists of Hubei Province (China) (2019CFA057).
Data Availability Statement: Not applicable.
Acknowledgments: The authors would like to acknowledge the ofﬁcial website of the Government
of Canada (https://www.canada.ca/en.html (accessed on 21 January 2023)) for providing the tem-
perature, tide, and wind data. The ice draft data are available from the Ocean Networks Canada Data
Archive (https://data.oceannetworks.ca/ (accessed on 21 January 2023).
Conﬂicts of Interest: The authors declare no conﬂicts of interest.
Appendix A
Table A1. List of collected SAR images.
Acquisition
Date
Sensor
Flight Direction
Acquisition
Date
Sensor
Flight Direction
20181231
S1A
Descending
20200216
S1B
Ascending
20190104
S1B
Ascending
20200307
S1A
Descending
20190112
S1A
Descending
20200311
S1B
Ascending
20190116
S1B
Ascending
20200331
S1A
Descending
20190124
S1A
Descending
20200404
S1B
Ascending
20190128
S1B
Ascending
20200412
S1A
Descending
20190205
S1A
Descending
20200416
S1B
Ascending
20190209
S1B
Ascending
20210101
S1A
Descending
20190217
S1A
Descending
20210105
S1B
Ascending
20190221
S1B
Ascending
20210113
S1A
Descending
20190301
S1A
Descending
20210117
S1B
Ascending
20190305
S1B
Ascending
20210125
S1A
Descending
20190313
S1A
Descending
20210129
S1B
Ascending
20190317
S1B
Ascending
20210206
S1A
Descending
20190325
S1A
Descending
20210210
S1B
Ascending
20190410
S1B
Ascending
20210218
S1A
Descending
20190406
S1A
Descending
20210222
S1B
Ascending
20190422
S1B
Ascending
20210302
S1A
Descending
20190430
S1A
Descending
20210306
S1B
Ascending
20190504
S1B
Ascending
20210314
S1A
Descending
20200107
S1A
Descending
20210318
S1B
Ascending
20200111
S1B
Ascending
20210326
S1A
Descending
20200119
S1A
Descending
20210330
S1B
Ascending
20200123
S1B
Ascending
20210407
S1A
Descending
20200131
S1A
Descending
20210411
S1B
Ascending
20200204
S1B
Ascending
20210419
S1A
Descending
20200212
S1A
Descending
20210423
S1B
Ascending

Remote Sens. 2023, 15, 1296
19 of 25
Figure A1. Perpendicular and temporal baselines of selected interferometric pairs. The red star de-
ﬁnes the master image, while the blue stars are slave images: (a) 2019 ascending, (b) 2020 descending,
(c) 2021 ascending, (d) 2019 descending, (e) 2020 ascending, and (f) 2021 descending.
Figure A2. Average coherence histogram: (a) 2019 ascending, (b) 2020 descending, (c) 2021 ascending,
(d) 2019 descending, (e) 2020 ascending, and (f) 2021 descending.

Remote Sens. 2023, 15, 1296
20 of 25
Figure A3. Filtered interferograms from ascending [(a–c): 2019, (d–f): 2020, and (g–i): 2021].
Figure A4. Time-series LOS deformation retrieved from the 2019 ascending images: the top-right
number of subﬁgures (a–j) represents the corresponding SAR images acquisition dates.

Remote Sens. 2023, 15, 1296
21 of 25
Figure A5. Time-series LOS deformation retrieved from the 2020 ascending images: the top-right
number of subﬁgures (a–h) represents the corresponding SAR images acquisition dates.
Figure A6. Time-series LOS deformation retrieved from the 2021 ascending images: the top-right
number of subﬁgures (a–j) represents the corresponding SAR images acquisition dates.

Remote Sens. 2023, 15, 1296
22 of 25
Figure A7. Time-series LOS deformation retrieved from the 2019 descending images: the top-right
number of subﬁgures (a–j) represents the corresponding SAR images acquisition dates.
Figure A8. Time-series LOS deformation retrieved from the 2020 descending images: the top-right
number of subﬁgures (a–i) represents the corresponding SAR images acquisition dates.

Remote Sens. 2023, 15, 1296
23 of 25
Figure A9. Time-series LOS deformation retrieved from the 2021 descending images: the top-right
number of subﬁgures (a–j) represents the corresponding SAR images acquisition dates.
References
1.
Laxon, S.W.; Giles, K.A.; Ridout, A.L.; Wingham, D.J.; Willatt, R.; Cullen, R.; Kwok, R.; Schweiger, A.; Zhang, J.; Haas, C.; et al.
CryoSat-2 estimates of Arctic sea ice thickness and volume. Geophys. Res. Lett. 2013, 40, 732–737. [CrossRef]
2.
Screen, J.A.; Simmonds, I. The central role of diminishing sea ice in recent Arctic temperature ampliﬁcation.
Nature 2010,
464, 1334–1337. [CrossRef] [PubMed]
3.
Barry, R.; Moritz, R.E.; Rogers, J. The fast ice regimes of the Beaufort and Chukchi Sea coasts, Alaska. Cold Reg. Sci. Technol. 1979,
1, 129–152. [CrossRef]
4.
Howell, S.E.; Laliberté, F.; Kwok, R.; Derksen, C.; King, J. Landfast ice thickness in the Canadian Arctic Archipelago from
observations and models. Cryosphere 2016, 10, 1463–1475. [CrossRef]
5.
Yu, Y.; Stern, H.; Fowler, C.; Fetterer, F.; Maslanik, J. Interannual variability of Arctic landfast ice between 1976 and 2007. J. Clim.
2014, 27, 227–243. [CrossRef]
6.
Krupnik, I.; Aporta, C.; Gearheard, S.; Laidler, G.J.; Holm, L.K. SIKU: Knowing Our Ice; Springer: Berlin/Heidelberg, Germany,
2010; Volume 595.
7.
Eicken, H.; Lovecraft, A.L.; Druckenmiller, M.L. Sea-ice system services: A framework to help identify and meet information
needs relevant for Arctic observing networks. Arctic 2009, 62, 119–136. [CrossRef]
8.
Pachauri, R.K.; Allen, M.R.; Barros, V.R.; Broome, J.; Cramer, W.; Christ, R.; Church, J.A.; Clarke, L.; Dahe, Q.; Dasgupta, P.; et al.
Climate Change 2014: Synthesis Report. Contribution of Working Groups I, II and III to the Fifth Assessment Report of the Intergovernmental
Panel on Climate Change; IPCC: Geneva, Switzerland, 2014.
9.
Ford, J.D.; Pearce, T.; Gilligan, J.; Smit, B.; Oakes, J. Climate change and hazards associated with ice use in northern Canada. Arct.
Antarct. Alp. Res. 2008, 40, 647–659. [CrossRef]
10.
Lovecraft, A.L.; Eicken, H. North by 2020: Perspectives on Alaska’s Changing Social-Ecological Systems; University of Alaska Press:
Fairbanks, AK, USA, 2011.
11.
Mesher, D.; Proskin, S.; Madsen, E. Ice road assessment, modeling and management. In Proceedings of the 7th International
Conference on Managing Pavements and Other Roadway Assets, Calgary, AI, Canada, 23–28 June 2008.
12.
Siitam, L.; Sipelgas, L.; Pärn, O.; Uiboupin, R. Statistical characterization of the sea ice extent during different winter scenarios in
the Gulf of Riga (Baltic Sea) using optical remote-sensing imagery. Int. J. Remote Sens. 2017, 38, 617–638. [CrossRef]
13.
Han, Y.; Liu, Y.; Hong, Z.; Zhang, Y.; Yang, S.; Wang, J. Sea ice image classiﬁcation based on heterogeneous data fusion and deep
learning. Remote Sens. 2021, 13, 592. [CrossRef]

Remote Sens. 2023, 15, 1296
24 of 25
14.
Ressel, R.; Frost, A.; Lehner, S. A neural network-based classiﬁcation for sea ice types on X-band SAR images. IEEE J. Sel. Top.
Appl. Earth Obs. Remote Sens. 2015, 8, 3672–3680. [CrossRef]
15.
Li, M.; Zhou, C.; Chen, X.; Liu, Y.; Li, B.; Liu, T. Improvement of the feature tracking and patter matching algorithm for sea ice
motion retrieval from SAR and optical imagery. Int. J. Appl. Earth Obs. Geoinf. 2022, 112, 102908. [CrossRef]
16.
Zhang, X.; Dierking, W.; Zhang, J.; Meng, J.; Lang, H. Retrieval of the thickness of undeformed sea ice from simulated C-band
compact polarimetric SAR images. Cryosphere 2016, 10, 1529–1545. [CrossRef]
17.
Cooke, C.L.; Scott, K.A. Estimating sea ice concentration from SAR: Training convolutional neural networks with passive
microwave data. IEEE Trans. Geosci. Remote Sens. 2019, 57, 4735–4747. [CrossRef]
18.
Fors, A.S.; Brekke, C.; Gerland, S.; Doulgeris, A.P.; Beckers, J.F. Late summer Arctic sea ice surface roughness signatures in C-band
SAR data. IEEE J. Sel. Top. Appl. Earth Obs. Remote Sens. 2015, 9, 1199–1215. [CrossRef]
19.
Muhuri, A.; Manickam, S.; Bhattacharya, A.; Snehmani. Snow cover mapping using polarization fraction variation with temporal
RADARSAT-2 C-band full-polarimetric SAR data over the Indian Himalayas. IEEE J. Sel. Top. Appl. Earth Obs. Remote Sens. 2018,
11, 2192–2209. [CrossRef]
20.
Qiao, H.; Zhang, P.; Li, Z.; Liu, C. A New Geostationary Satellite-Based Snow Cover Recognition Method for FY-4A AGRI. IEEE J.
Sel. Top. Appl. Earth Obs. Remote Sens. 2021, 14, 11372–11385. [CrossRef]
21.
Tsai, Y.L.S.; Dietz, A.; Oppelt, N.; Kuenzer, C. Remote sensing of snow cover using spaceborne SAR: A review. Remote Sens. 2019,
11, 1456. [CrossRef]
22.
Zhang, W.; Zhu, W.; Tian, X.; Zhang, Q.; Zhao, C.; Niu, Y.; Wang, C. Improved DEM reconstruction method based on multibaseline
InSAR. IEEE Geosci. Remote Sens. Lett. 2021, 19, 1–5. [CrossRef]
23.
Crosetto, M.; Solari, L.; Mróz, M.; Balasis-Levinsen, J.; Casagli, N.; Frei, M.; Oyen, A.; Moldestad, D.A.; Bateson, L.; Guerrieri, L.;
et al. The evolution of wide-area DInSAR: From regional and national services to the European Ground Motion Service. Remote
Sens. 2020, 12, 2043. [CrossRef]
24.
Li, S.; Shapiro, L.; Mcnutt, L.; Feffers, A. Application of satellite radar interferometry to the detection of sea ice deformation. J.
Remote Sens. Soc. Jpn. 1996, 16, 153–163.
25.
Wang, Z.; Liu, J.; Wang, J.; Wang, L.; Luo, M.; Wang, Z.; Ni, P.; Li, H. Resolving and analyzing landfast ice deformation by InSAR
technology combined with Sentinel-1A ascending and descending orbits data. Sensors 2020, 20, 6561. [CrossRef] [PubMed]
26.
Liu, J.; Hu, J.; Li, Z.; Sun, Q.; Ma, Z.; Zhu, J.; Wen, Y. Dynamic estimation of multi-dimensional deformation time series from Insar
based on Kalman ﬁlter and strain model. IEEE Trans. Geosci. Remote Sens. 2021, 60, 1–16. [CrossRef]
27.
Liu, J.; Hu, J.; Li, Z.; Ma, Z.; Wu, L.; Jiang, W.; Feng, G.; Zhu, J. Complete three-dimensional coseismic displacements due to
the 2021 Maduo earthquake in Qinghai Province, China from Sentinel-1 and ALOS-2 SAR images. Sci. China Earth Sci. 2022,
65, 687–697. [CrossRef]
28.
Xing, X.; Chang, H.C.; Chen, L.; Zhang, J.; Yuan, Z.; Shi, Z. Radar interferometry time series to investigate deformation of soft
clay subgrade settlement—A case study of Lungui Highway, China. Remote Sens. 2019, 11, 429. [CrossRef]
29.
Berardino, P.; Fornaro, G.; Lanari, R.; Sansosti, E. A new algorithm for surface deformation monitoring based on small baseline
differential SAR interferograms. IEEE Trans. Geosci. Remote Sens. 2002, 40, 2375–2383. [CrossRef]
30.
Hu, J.; Li, Z.; Ding, X.; Zhu, J.; Zhang, L.; Sun, Q. 3D coseismic displacement of 2010 Darﬁeld, New Zealand earthquake estimated
from multi-aperture InSAR and D-InSAR measurements. J. Geod. 2012, 86, 1029–1041. [CrossRef]
31.
Poland, M.P.; Zebker, H.A. Volcano geodesy using InSAR in 2020: The past and next decades. Bull. Volcanol. 2022, 84, 27.
[CrossRef]
32.
Wang, R.; Yang, M.; Dong, J.; Liao, M. Investigating deformation along metro lines in coastal cities considering different structures
with InSAR and SBM analyses. Int. J. Appl. Earth Obs. Geoinf. 2022, 115, 103099. [CrossRef]
33.
Wang, Y.; Feng, G.; Li, Z.; Xu, W.; Zhu, J.; He, L.; Xiong, Z.; Qiao, X. Retrieving the displacements of the Hutubi (China)
underground gas storage during 2003–2020 from multi-track InSAR. Remote Sens. Environ. 2022, 268, 112768. [CrossRef]
34.
Shugar, D.H.; Jacquemart, M.; Shean, D.; Bhushan, S.; Upadhyay, K.; Sattar, A.; Schwanghart, W.; McBride, S.; De Vries, M.V.W.;
Mergili, M.; et al.
A massive rock and ice avalanche caused the 2021 disaster at Chamoli, Indian Himalaya.
Science 2021,
373, 300–306. [CrossRef]
35.
Zhang, F.; Pang, X.; Lei, R.; Zhai, M.; Zhao, X.; Cai, Q. Arctic sea ice motion change and response to atmospheric forcing between
1979 and 2019. Int. J. Climatol. 2022, 42, 1854–1876. [CrossRef]
36.
Lanari, R.; Casu, F.; Manzo, M.; Lundgren, P. Application of the SBAS-DInSAR technique to fault creep: A case study of the
Hayward fault, California. Remote Sens. Environ. 2007, 109, 20–28. [CrossRef]
37.
Arangio, S.; Calò, F.; Di Mauro, M.; Bonano, M.; Marsella, M.; Manunta, M. An application of the SBAS-DInSAR technique for the
assessment of structural damage in the city of Rome. Struct. Infrastruct. Eng. 2014, 10, 1469–1483. [CrossRef]
38.
Béjar-Pizarro, M.; Ezquerro, P.; Herrera, G.; Tomás, R.; Guardiola-Albert, C.; Hernández, J.M.R.; Merodo, J.A.F.; Marchamalo,
M.; Martínez, R. Mapping groundwater level and aquifer storage variations from InSAR measurements in the Madrid aquifer,
Central Spain. J. Hydrol. 2017, 547, 678–689. [CrossRef]
39.
Pritt, M.D. Phase unwrapping by means of multigrid techniques for interferometric SAR. IEEE Trans. Geosci. Remote Sens. 1996,
34, 728–738. [CrossRef]
40.
Strang, G. Orthogonality: projections and least squares approximations. In Linear Algebra and its Applications; Harcourt Brace
Jovanovich College Publishers: Orlando, USA, 1988; pp. 153–162.

Remote Sens. 2023, 15, 1296
25 of 25
41.
Golub, G.H.; Van Loan, C.F. Matrix Computations; JHU Press: Baltimore, MD, USA, 2013.
42.
Zhao, R.; Li, Z.w.; Feng, G.c.; Wang, Q.j.; Hu, J. Monitoring surface deformation over permafrost with an improved SBAS-InSAR
algorithm: With emphasis on climatic factors modeling. Remote Sens. Environ. 2016, 184, 276–287. [CrossRef]
43.
Pepe, A.; Solaro, G.; Calo, F.; Dema, C. A minimum acceleration approach for the retrieval of multiplatform InSAR deformation
time series. IEEE J. Sel. Top. Appl. Earth Obs. Remote Sens. 2016, 9, 3883–3898. [CrossRef]
44.
Fialko, Y.; Simons, M.; Agnew, D. The complete (3-D) surface displacement ﬁeld in the epicentral area of the 1999 Mw7. 1 Hector
Mine earthquake, California, from space geodetic observations. Geophys. Res. Lett. 2001, 28, 3063–3066. [CrossRef]
45.
Hu, J.; Li, Z.; Zhu, J.; Ren, X.; Ding, X. Inferring three-dimensional surface displacement ﬁeld by combining SAR interferometric
phase and amplitude information of ascending and descending orbits. Sci. China Earth Sci. 2010, 53, 550–560. [CrossRef]
46.
Huebert, R. Climate change and Canadian sovereignty in the Northwest Passage. In The Calgary Papers in Military and Strategic
Studies; 2011. Available online: https://cjc-rcc.ucalgary.ca/index.php/cpmss/article/view/36337 (accessed on 22 February 2023).
47.
Tivy, A.; Howell, S.E.; Alt, B.; McCourt, S.; Chagnon, R.; Crocker, G.; Carrieres, T.; Yackel, J.J. Trends and variability in summer
sea ice cover in the Canadian Arctic based on the Canadian Ice Service Digital Archive, 1960–2008 and 1968–2008. J. Geophys. Res.
Ocean. 2011, 116, 1–25.
48.
Torres, R.; Snoeij, P.; Geudtner, D.; Bibby, D.; Davidson, M.; Attema, E.; Potin, P.; Rommen, B.; Floury, N.; Brown, M.; et al. GMES
Sentinel-1 mission. Remote Sens. Environ. 2012, 120, 9–24. [CrossRef]
49.
Touzi, R.; Lopes, A.; Bruniquel, J.; Vachon, P.W. Coherence estimation for SAR imagery. IEEE Trans. Geosci. Remote Sens. 1999,
37, 135–149. [CrossRef]
50.
van der Sanden, J.J.; Short, N.; Drouin, H. InSAR coherence for automated lake ice extent mapping: TanDEM-X bistatic and
pursuit monostatic results. Int. J. Appl. Earth Obs. Geoinf. 2018, 73, 605–615. [CrossRef]
51.
Wang, S.; Lu, X.; Chen, Z.; Zhang, G.; Ma, T.; Jia, P.; Li, B. Evaluating the Feasibility of illegal open-pit mining identiﬁcation using
insar coherence. Remote Sens. 2020, 12, 367. [CrossRef]
52.
Xing, X.; Zhang, T.; Chen, L.; Yang, Z.; Liu, X.; Peng, W.; Yuan, Z. InSAR Modeling and Deformation Prediction for Salt Solution
Mining Using a Novel CT-PIM Function. Remote Sens. 2022, 14, 842. [CrossRef]
53.
Xing, X.; Zhu, Y.; Xu, W.; Peng, W.; Yuan, Z. Measuring Subsidence Over Soft Clay Highways Using a Novel Time-Series InSAR
Deformation Model With an Emphasis on Rheological Properties and Environmental Factors (NREM). IEEE Trans. Geosci. Remote
Sens. 2022, 60, 1–19. [CrossRef]
54.
Dammann, D.O.; Eriksson, L.E.; Mahoney, A.R.; Eicken, H.; Meyer, F.J. Mapping pan-Arctic landfast sea ice stability using<?
xmltex\break?> Sentinel-1 interferometry. Cryosphere 2019, 13, 557–577.
55.
Zhu, Y.; Yao, X.; Yao, L.; Yao, C. Detection and characterization of active landslides with multisource SAR data and remote
sensing in western Guizhou, China. Nat. Hazards 2022, 111, 973–994. [CrossRef]
56.
Xing, X.; Chen, L.; Yuan, Z.; Shi, Z. An improved time-series model considering rheological parameters for surface deformation
monitoring of soft clay subgrade. Sensors 2019, 19, 3073. [CrossRef]
57.
Marbouti, M.; Praks, J.; Antropov, O.; Rinne, E.; Leppäranta, M. A study of landfast ice with Sentinel-1 repeat-pass interferometry
over the Baltic Sea. Remote Sens. 2017, 9, 833. [CrossRef]
58.
Chen, Z.; Montpetit, B.; Banks, S.; White, L.; Behnamian, A.; Duffe, J.; Pasher, J. Insar monitoring of arctic landfast sea ice
deformation using l-band alos-2, c-band radarsat-2 and sentinel-1. Remote Sens. 2021, 13, 4570. [CrossRef]
59.
Chaves-Barquero, L.G.; Luong, K.H.; Mundy, C.; Knapp, C.W.; Hanson, M.L.; Wong, C.S. The release of wastewater contaminants
in the Arctic: A case study from Cambridge Bay, Nunavut, Canada. Environ. Pollut. 2016, 218, 542–550. [CrossRef] [PubMed]
60.
Goldstein, R.V.; Osipenko, N.M.; Leppäranta, M. Relaxation scales and the structure of fractures in the dynamics of sea ice. Cold
Reg. Sci. Technol. 2009, 58, 29–35. [CrossRef]
61.
Rignot, E.; Steffen, K. Channelized bottom melting and stability of ﬂoating ice shelves. Geophys. Res. Lett. 2008, 35, 1–5. [CrossRef]
62.
Berg, A.; Dammert, P.; Eriksson, L.E. X-band interferometric SAR observations of Baltic fast ice. IEEE Trans. Geosci. Remote Sens.
2014, 53, 1248–1256. [CrossRef]
Disclaimer/Publisher’s Note: The statements, opinions and data contained in all publications are solely those of the individual
author(s) and contributor(s) and not of MDPI and/or the editor(s). MDPI and/or the editor(s) disclaim responsibility for any injury to
people or property resulting from any ideas, methods, instructions or products referred to in the content.