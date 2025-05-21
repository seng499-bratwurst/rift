Atmosphere-Ocean
ISSN: 0705-5900 (Print) 1480-9214 (Online) Journal homepage: www.tandfonline.com/journals/tato20
Eﬀects of Seasonal Ice Coverage on the Physical
Oceanographic Conditions of the Kitikmeot Sea in
the Canadian Arctic Archipelago
Chengzhu Xu, Wahad Mikhael, Paul G. Myers, Brent Else, Richard P. Sims &
Qi Zhou
To cite this article: Chengzhu Xu, Wahad Mikhael, Paul G. Myers, Brent Else, Richard P. Sims &
Qi Zhou (2021) Eﬀects of Seasonal Ice Coverage on the Physical Oceanographic Conditions of
the Kitikmeot Sea in the Canadian Arctic Archipelago, Atmosphere-Ocean, 59:4-5, 214-232, DOI:
10.1080/07055900.2021.1965531
To link to this article:  https://doi.org/10.1080/07055900.2021.1965531
Published online: 31 Aug 2021.
Submit your article to this journal 
Article views: 406
View related articles 
View Crossmark data
Citing articles: 4 View citing articles 
Full Terms & Conditions of access and use can be found at
https://www.tandfonline.com/action/journalInformation?journalCode=tato20

Effects of Seasonal Ice Coverage on the Physical
Oceanographic Conditions of the Kitikmeot Sea in the
Canadian Arctic Archipelago
Chengzhu Xu1, Wahad Mikhael2, Paul G. Myers3, Brent Else4, Richard P. Sims4, and
Qi Zhou
2*
1Bedford Institute of Oceanography, Fisheries and Oceans Canada, Dartmouth, Nova Scotia,
Canada
2Department of Civil Engineering, University of Calgary, Calgary, Alberta, Canada
3Department of Earth and Atmospheric Sciences, University of Alberta, Edmonton, Alberta,
Canada
4Department of Geography, University of Calgary, Calgary, Alberta, Canada
[Original manuscript received 22 October 2020; accepted 15 July 2021]
ABSTRACT
The Kitikmeot Sea is a semi-enclosed, east–west waterway in the southern Canadian Arctic Archi-
pelago (CAA). In the present work, the ice conditions, stratiﬁcation, and circulation of the Kitikmeot Sea are diag-
nosed using numerical simulations with a 1/12° resolution. The physical oceanographic conditions of the
Kitikmeot Sea are different from channels in the northern CAA due to the existence of a substantial ice-free
period each year. The consequences of such ice conditions are twofold. First, through ﬂuctuations of external for-
cings, such as solar radiation and wind stress, acting directly or indirectly on the sea surface, the seasonal ice
coverage leads to signiﬁcant seasonal variations in both stratiﬁcation and circulation. Our simulation results
suggest that such variations include freshening and deepening of the surface layer, in which salinity can reach
as low as 15 during the peak runoff season, and signiﬁcantly stronger along-shore currents driven directly by
the wind stress during the ice-free season. The second consequence is that the sea ice is not landfast but can
move freely during the melting season. By analyzing the relative importance of thermodynamic (freezing and/
or melting) and dynamic (ice movement) processes to the ice dynamics, our simulation results suggest that
there is a net inﬂow of sea ice into the Kitikmeot Sea, which melts locally each summer. The movement of sea
ice thus provides a signiﬁcant freshwater pathway, which contributes approximately 14 km3 yr−1 of fresh
water to the Kitikmeot Sea, on average, equivalent to a third of freshwater input from runoff from the land.
RÉSUMÉ
[Traduit par la rédaction] La mer de Kitikmeot est une voie navigable semi-fermée, orientée est-ouest,
située dans le sud de l’archipel Arctique canadien (AAC). Dans le présent travail, les conditions de glace, la stra-
tiﬁcation et la circulation de la mer de Kitikmeot sont diagnostiquées au moyen de simulations numériques avec
une résolution de 1/12°. Les conditions océanographiques physiques de la mer de Kitikmeot sont différentes de
celles des canaux du nord de l’AAC en raison de l’existence d’une importante période sans glace chaque
année. Les conséquences de ces conditions de glace sont doubles. Premièrement, à travers les ﬂuctuations des for-
çages externes, tels que le rayonnement solaire et le stress éolien, agissant directement ou indirectement sur la
surface de la mer, la couverture de glace saisonnière entraîne des variations saisonnières signiﬁcatives à la
fois dans la stratiﬁcation et la circulation. Les résultats de nos simulations donnent à penser que ces variations
comprennent le rafraîchissement et l’approfondissement de la couche de surface, dans laquelle la salinité peut des-
cendre jusqu’à 15 pendant le pic de la saison de ruissellement, et des courants littoraux nettement plus forts,
entraînés directement par la contrainte du vent pendant la saison sans glace. La deuxième conséquence est que
la glace de mer n’est pas une terre ferme mais peut se déplacer librement pendant la saison de fonte. En analysant
l’importance relative des processus thermodynamiques (congélation et/ou fonte) et dynamiques (mouvement de la
glace) dans la dynamique de la glace, les résultats de notre simulation suggèrent qu’il existe un afﬂux net de glace
de mer dans la mer de Kitikmeot, qui fond localement chaque été. Le mouvement de la glace de mer constitue donc
une importante voie d’accès à l’eau douce, qui apporte environ 14 km3 par an−1 d’eau douce à la mer de Kitikmeot,
en moyenne, soit l’équivalent d’un tiers de l’apport d’eau douce provenant du ruissellement terrestre.
KEYWORDS
numerical modelling; Canadian Arctic Archipelago; Kitikmeot Sea; seasonal oceanographic
conditions
*Corresponding author’s email: qi.zhoul@ucalgary.ca; qi.zhou1@ucalgary.ca
ATMOSPHERE-OCEAN 59 (4–5) 2021, 214–232
https://doi.org/10.1080/07055900.2021.1965531
La Société canadienne de météorologie et d’océanographie

1 Introduction
The Kitikmeot Sea is a semi-enclosed, east–west waterway,
located in the southern Canadian Arctic Archipelago (CAA)
between Victoria Island and mainland Canada (Fig. 1a). It
consists of Coronation Gulf (CG) and Queen Maud Gulf
(QMG) and is bounded to the northwest by Dolphin and
Union Strait (DUS) and to the northeast by Victoria Strait
(VS). The deepest part of CG is over 300 m, whereas QMG
is relatively shallower with an average depth of approximately
100 m. The two gulfs are connected to each other through
Dease Strait (DS) near Cambridge Bay. The existence of rela-
tively longer ice-free seasons makes the Kitikmeot Sea the
practical route for vessels traversing the Northwest Passage.
Located near the centre of the Kitikmeot Sea, the coastal com-
munity of Cambridge Bay is the largest settlement in this
region. In recent years, there have been signiﬁcant research
efforts in the Cambridge Bay area. Ocean Networks Canada
(ONC) has established an underwater cabled observatory at
Cambridge Bay collecting data continuously in near real-
time, throughout the year. The Canadian High Arctic
Research Station, operated by Polar Knowledge Canada, is
also located at Cambridge Bay. In addition, the Kitikmeot
Sea Science Study has been carried out in this region (Wil-
liams et al., 2018).
For the purpose of our analysis, we deﬁne the Kitikmeot
Sea as the water body within the open boundaries indicated
by the two red lines in Fig. 1b. The surface area of the
water body within these boundaries is approximately 6 ×
104 km2, while the total volume is approximately 4 ×
103 km3. The physical oceanographic conditions of the Kitik-
meot Sea are unique, primarily a result of the large amount of
river runoff (relative to the size of the area) and the shallow
bounding sills at both ends (less than 30 m deep at the shal-
lowest points of both DUS and VS). Most notably, the
average salinity is much lower than that of most of the
Arctic Ocean, and the circulation near the bounding sills is
estuarine-like (Williams et al., 2018).
Although there have been signiﬁcant research efforts in the
Cambridge Bay area in recent years, comprehensive studies of
the entire Kitikmeot Sea are still lacking. In particular, the
extent to which the physical oceanographic conditions of
the Kitikmeot Sea vary on a seasonal time scale is less
clear. The physical oceanographic conditions of the Kitikmeot
Sea are different from channels in the northern CAA due to
the existence of a substantial ice-free period each year.
While multi-year ice covers the majority of the northern
CAA, ﬁrst-year ice dominates in CG and DS (Michel et al.,
2015) though interannual variability also occurs (Howell
et al., 2015). The seasonal ice coverage is expected to
produce signiﬁcant seasonal variation in terms of both strati-
ﬁcation and circulation through ﬂuctuations of external for-
cings, such as solar radiation and wind stress, acting
directly or indirectly on the sea surface. The movement of
sea ice during the melting season could also provide a poten-
tial freshwater pathway and affect the distribution of surface
salinity. Moreover, Arctic rivers also exhibit large seasonal
variation in discharge (McLaughlin et al., 2004). These
rivers are usually frozen solid for most of the year but
exhibit extreme runoff events for a brief time in the
summer. These runoff events could signiﬁcantly affect the
salinity, and hence the stratiﬁcation, of the Kitikmeot Sea
on a relatively short time scale.
The ecology and biogeochemistry of the Kitikmeot Sea is
unique within the CAA, likely a result of the unique con-
ditions imposed by bounding sills, high freshwater content,
and signiﬁcant stratiﬁcation. Back et al. (2021) found that
the Kitikmeot Sea is perhaps the most nitrogen-depleted
system in the Arctic Ocean largely due to the inhibition of ver-
tical mixing by stratiﬁcation. This nutrient limitation signiﬁ-
cantly
curtails
primary
production
although
enhanced
vertical mixing over shallow sills has been observed to
create “invisible polynyas”—small areas of thin ice and
high biological productivity (Dalman et al., 2019). In a
survey of Arctic cod (Boreogadus saida) abundance in the
CAA, Bouchard et al. (2018) found a near absence of the
key forage ﬁsh in the Kitikmeot Sea, which they associated
with low prey availability (in turn linked to low primary pro-
duction), shallow water, and slow circulation. As a result,
many marine mammals that are endemic to the CAA (e.g.,
beluga whales, narwhals, and polar bears) are rarely observed
in the region. On the other hand, the low-salinity water pro-
vides an excellent summer habitat for Arctic char, which
favour estuarine conditions during their annual migration to
the marine environment (Harris et al., 2020). Stratiﬁcation
and high freshwater content also strongly affect biogeochem-
istry; for example, CG is a key region of the CAA that is
observed to act as a source of CO2 to the atmosphere during
the ice-free season (Ahmed et al., 2019), likely a result of
the strong river discharge and high summer sea surface temp-
erature. In a rapidly changing climate, the ecosystem of the
Cambridge Bay area has been under emerging pressures
from environmental changes and human activities (e.g.,
reduced ice coverage and increased shipping activities; Falar-
deau-Côté, 2020).
To date, most research efforts in the Kitikmeot Sea have
focused on ocean observations, but the data collected tend
to be sparse in space and/or time. Modelling studies, on the
other hand, can provide insights into the physical oceano-
graphic conditions on a larger spatial and temporal scale,
which are essential for a better understanding of the local eco-
system, in particular carbon cycling, nutrient transport, and
primary production. In the present work, results of numerical
simulations with a 1/12° resolution are diagnosed in order to
better understand the physical oceanographic conditions of
the Kitikmeot Sea, particularly the implications of the seaso-
nal ice coverage on stratiﬁcation and circulation. The remain-
der of this paper is organized as follows. The numerical model
and methods of analysis are described in Section 2. The model
is evaluated in Section 3 by comparing model output with
available observational data. The simulation results are pre-
sented in Section 4, focusing on the ice conditions,
Effects of Seasonal Ice Coverage on the Kitikmeot Sea / 215
ATMOSPHERE-OCEAN 59 (4–5) 2021, 214–232
https://doi.org/10.1080/07055900.2021.1965531
Canadian Meteorological and Oceanographic Society

stratiﬁcation, and ocean circulation. The implications of the
physical oceanographic conditions on the local ecosystem
and biogeochemistry are discussed in Section 5. Finally, ﬁnd-
ings of this work are summarized in Section 6.
2 Methods
a Model Description
The numerical model used in this study is the Nucleus for
European Modelling of the Ocean, version 3.4 (NEMO;
Madec and the NEMO team, 2008) coupled with the
Louvain-la-Neuve Sea Ice Model (LIM), version 2 (Hunke
and Dukowicz, 1997). The model is conﬁgured for the
Arctic and northern hemisphere Atlantic at 1/12° resolution
(ANHA12), with open boundaries located at Bering Strait
and 20°S at the Atlantic Ocean. The model grid is extracted
from the ORCA12 tri-polar grid (Drakkar Group, 2007), in
which one of the mesh North Poles is located directly to the
south of the Kitikmeot Sea. With this grid conﬁguration, the
model has the lowest resolution near the equator but the
highest horizontal resolution within the Kitikmeot Sea,
which is about 2 km in both zonal and meridional directions,
as shown in Fig. 1b. In the vertical direction, there are 50
levels with layer thickness smoothly increasing from approxi-
mately 1 m at the surface to approximately 450 m at the
bottom. Enhanced resolution is applied to the surface layer,
with less than 2 m vertical resolution for the top 10 m. The
top 100 m is covered by 22 vertical levels, and the deepest
location of the Kitikmeot Sea is covered by 30 levels.
Partial steps (Bernard et al., 2006) are utilized to better
resolve the ocean ﬂoor. Further detail on the ANHA12 con-
ﬁguration can be found in Hu et al. (2018).
The input data of the model are summarized in Table 1. The
topography of the sea ﬂoor in the model is interpolated from
bathymetry data collected by W. H. Smith and Sandwell
Fig. 1
(a) Bathymetry map of the Kitikmeot Sea. (b) ANHA12 grid resolution in the Kitikmeot Sea. Pseudo-colour map shows the area covered by each hori-
zontal grid box. Mesh grid shows the distribution of every ﬁve horizontal grid points. Red lines in (b) indicate the two open boundaries of the Kitikmeot
Sea.
TABLE 1.
Summary of input data.
Input data
Data Source
Spatial
Resolution
Temporal
Resolution
Topography
W. H. Smith and
Sandwell (1997)
1–12 km
Not
applicable
ETOPO1 (Amante
and Eakins, 2009)
1/60°
Not
applicable
Initial conditions
GLORYS2v3 (Masina
et al., 2017)
1/4°
Not
applicable
Open boundary
conditions
GLORYS2v3 (Masina
et al., 2017)
1/4°
Monthly
Atmospheric
forcings
CGRF (G. C. Smith
et al., 2014)
33 km
Hourly
River discharge
Dai et al. (2009)
1°
Monthly
Greenland
meltwater
Bamber et al. (2012)
5 km
Monthly
216 / Chengzhu Xu et al.
ATMOSPHERE-OCEAN 59 (4–5) 2021, 214–232
https://doi.org/10.1080/07055900.2021.1965531
La Société canadienne de météorologie et d’océanographie

(1997) and the ETOPO1 Global Relief Model (Amante and
Eakins, 2009). Although these datasets are the best available
datasets for the model bathymetry, the lack of small-scale fea-
tures might be a concern. In particular, because the model was
originally set up for studying large-scale Arctic dynamics,
several narrow and shallow regions are not represented
including southern Bathurst Inlet and the water body to the
south of Kent Peninsula. Long-term studies of the Kitikmeot
Sea will need to carefully consider all details of the local
topography.
The simulations are initialized on 1 January 2002. The
initial and open boundary conditions including temperature,
salinity, zonal and meridional velocities, sea surface height,
and sea-ice ﬁelds, are interpolated from the 1/4° Global
Ocean Reanalysis and Simulations, stream 2, version 3 pro-
duced by Mercator Ocean (Masina et al., 2017). The atmos-
pheric forcings, including wind stress, precipitation, air
temperature, speciﬁc humidity, shortwave and longwave radi-
ation, are interpolated from the Canadian Meteorological
Centre’s Global Deterministic Prediction System Reforecasts
(CGRF) dataset with a temporal resolution of 1 h and a spatial
resolution of 33 km (G. C. Smith et al., 2014). The freshwater
input from the continent is remapped from interannual
monthly 1° resolution river discharge data (Dai et al., 2009)
and 5 km resolution Greenland meltwater data (Bamber
et al., 2012) onto the model grid. For study periods beyond
the coverage of the original datasets (i.e., since 2007 for
river discharge and 2010 for Greenland meltwater), available
source data from the latest years are adopted. Because of the
considerable volume of output data, the output is written out
as ﬁve-day time-averaged data.
When the model was originally set up, no tidal forcing was
included. Based on data presented in Padman and Erofeeva
(2004), the mean tidal current speed is generally small in
the Kitikmeot Sea (≏1 cm s−1) except for regions near
shallow sills, such as VS (≏10 cm s−1). Although the
overall circulation is not likely to be affected dramatically,
this still poses a major limitation in terms of inﬂow and
outﬂow through the bounding sills, and future experiments
will need to include explicit tidal forcing.
b Calculations of Ice Thickness Changes
The ice model is coupled to the ocean model at every model
time step. It calculates ice dynamics due to both thermodyn-
amic (freezing and/or melting) and dynamic (transport) pro-
cesses (Fichefet and Maqueda, 1997). Although these two
processes occur simultaneously in the real world, they can
be decoupled in the model, so that the relative importance
of each can be better understood. In practice, we will assess
the importance of each process in terms of change in ice thick-
ness with respect to time. Let H denote the total thickness of
the sea ice, the decomposition can be written as
dH
dt = d
dt (Hthermal + Hdynamic),
(1)
where Hthermal and Hdynamic denote the contribution of the ther-
modynamic and dynamic processes, respectively, to the
overall change in ice thickness.
The fact that sea ice is not necessarily always landfast
means that the movement of sea ice provides a potential
pathway for freshwater transport. To quantify the net inﬂow
of sea ice into the Kitikmeot Sea, we deﬁne the volume ﬂux
of sea ice through a particular channel cross-section,
denoted by Qice, as
Qice =
b
a
HCiceviceds,
(2)
where a and b are the two endpoints of the channel cross-
section, Cice is the ice concentration, vice is the magnitude
of ice velocity perpendicular to the cross-section with the
positive direction pointing toward the Kitikmeot Sea (so
that inﬂow is positive and outﬂow is negative), and ds is
deﬁned along the cross-section. In our calculation, the two
cross-sections are chosen to be within DUS and VS, as indi-
cated by the red lines in Fig. 1b.
c Calculations of Freshwater and Heat Contents
Following Hu et al. (2019), the freshwater content (FWC) of
the Kitikmeot Sea is deﬁned as
FWC =
0
z1
Sref −S
Sref
dz,
(3)
where S is the salinity from model output, Sref is the reference
salinity which is set to 34.8, and z1 is the lower limit of the
depth level of integration, which is chosen to be the 34.8 halo-
cline. The freshwater content is measured in metres. Given the
freshwater content, the total freshwater storage (i.e., volume)
can be calculated by integrating FWC over the entire surface
area of the Kitikmeot Sea, that is,
FWstorage =
 
A
FWCdA.
(4)
Following Myers and Ribergaard (2013), the heat content
(HC) per unit area is deﬁned as
HC = ρ0Cp
0
z2
(T −Tref)dz,
(5)
where T is the temperature from model output; Tref is the
reference temperature, which is set to 0°C; z2 is the lower
limit of the depth level of integration, which is chosen to be
40 m below the surface; ρ0 is the reference density, which
is set to 1022 kg m−3; and Cp is the speciﬁc heat of water
and is set to 4182 J K−1 kg−1.
Effects of Seasonal Ice Coverage on the Kitikmeot Sea / 217
ATMOSPHERE-OCEAN 59 (4–5) 2021, 214–232
https://doi.org/10.1080/07055900.2021.1965531
Canadian Meteorological and Oceanographic Society

3 Model evaluation
a Comparison with Data Collected by Ocean Networks
Canada
The model output is compared with two sets of observational
data measured in the Cambridge Bay area (Fig. 2). The ﬁrst
set of data was collected by ONC, which has established an
underwater observatory near the shore of Cambridge Bay
(indicated by the star in Fig. 2) at a depth of 8 m (Duke
et al., 2021). Because these data are measured in shallow
water inside a coastal bay, the small-scale features are chal-
lenging for even a higher-resolution model to represent. In
order for the model to represent the overall dynamics in this
region, time series of model data are produced by averaging
the model output over an area covering Cambridge Bay and
the surrounding water (indicated by the blue box in Fig. 2),
instead of a single point. We have tested different sizes and
locations of the box near Cambridge Bay and found that the
results are not sensitive to the particular sizes or locations
over which the model output data are averaged. Moreover,
the model data are averaged at the layer 8 m below the
surface in order to match the particular depth at which
ONC’s observatory is located.
The comparison between model output and data collected
from ONC’s observatory is shown by time series plots in
Fig. 3. For both datasets, ﬁve-day averaged values are
shown. The ﬁgure suggests that the model captures the
overall seasonal cycle for sea ice and water temperature
reasonably well. The timing for break-up and freeze-up of
sea ice in the model is consistent with observations, except
for 2015 (details of the ice conditions in 2014–2015 will be
discussed in Section 4.a). The observed ice thickness is
measured in terms of ice draft by ONC, and hence is slightly
lower than that estimated by the model. To adjust for this
difference, the original data are multiplied by 1.11 to
produce the red curve in Fig. 3a, assuming that the density
of ice is roughly 90% of the density of seawater (Timco and
Frederking, 1996). The temperature data also show good
agreement between the model and observations, with the
exception of summer 2015 when temperature estimated by
the model is lower than observed, which is likely due to the
excessive summer sea ice that reduces heat input in the model.
For salinity, Fig. 3c shows that from November to May the
model data and observational data are similar, but some issues
exist from May to November, when the salinity estimated by
the model is lower than that observed by the ONC observa-
tory. One of the contributing reasons for this difference, as
discussed in the previous section, is the lack of runoff data
for the model from 2007 onward, which affects the model’s
ability to accurately estimate the freshwater content in the
water body, especially in the summers. Moreover, the ONC
observatory is located extremely close to shore, but the
model has limited ability to accurately represent the dynamics
in this particular location. The comparison of density data is
similar to that of salinity data because the density in the
Arctic Ocean is determined primarily by salinity.
b Comparison with Data Collected on the R/V Martin
Bergmann
From 2015 to 2018, oceanographic conductivity, temperature,
depth (CTD) transects were conducted annually in the Kitik-
meot Sea on the R/V Martin Bergmann during the ﬁrst week
of August. The locations of measurements are indicated by
the red dots and denoted as “R1” to “R5” in Fig. 2; the transect
that connects these locations (i.e., the “R-transect”) is indi-
cated by the blue line. A Seabird Scientiﬁc SBE19plus CTD
measuring at 5 Hz was used to make the CTD measurements
except in 2016 when an RBR Concerto also measuring at 5 Hz
was substituted for the SBE19plus. Annual calibrations were
completed between each ﬁeld season at the Institute of Ocean
Sciences (Sidney, British Columbia, Canada). The descent
speed typically varied between 0.3 and 1 ms−1. Measurements
were interpolated down to 0.01 m and bin averaged to 0.1 m.
Figures 4 and 5 show comparisons of temperature and sal-
inity data between measurements collected at locations R1 to
R5 and model output along the transect connecting these
locations. Note that part of the difference between the model
output and measurements is a result of the model output
being averaged over a ﬁve-day period. Figure 4 shows that
the model correctly captures the salinity at least in the
surface layer (within approximately 40 m of depth), including
the signiﬁcant amount of surface fresh water in 2015 caused
by the delay in ice melting in that particular year. On the
other hand, in the bottom layer, the model estimation of sal-
inity is slightly higher than measured, while measurements
suggest that there is no signiﬁcant difference from the upper
layer. Figure 5 shows that the model-estimated temperature
matches the measured temperature qualitatively, except for
2015 when the model underestimated the surface temperature,
possibly because of excessive summer sea ice in the model. In
the other years, the model slightly overestimated the extent of
surface warm water and the bottom temperature.
To summarize, the comparison between model data and
measurements suggests that the model is able to represent
aspects of the large-scale circulation and hydrography reason-
ably well. Some errors might be caused by missing or parame-
terized processes, as well as issues with the initial conditions
and/or forcing (such as the lack of runoff data) and could be
reduced as the model improves in the future.
4 Simulation results
a Ice Conditions
The simulation results from 2011 to 2018 will be discussed in
this section, in which all data presented are from the model
output, unless otherwise speciﬁed. The total volume of the
sea ice within the Kitikmeot Sea enclosed by the boundaries
indicated by the red lines in Fig. 1b is shown as a time
series in Fig. 6. The annual volume of ice formed and
melted varies from 60 to 80 km3 from year to year. Unlike
the northern CAA which is ice covered during most of the
year, in the Kitikmeot Sea there is a signiﬁcant ice-free
218 / Chengzhu Xu et al.
ATMOSPHERE-OCEAN 59 (4–5) 2021, 214–232
https://doi.org/10.1080/07055900.2021.1965531
La Société canadienne de météorologie et d’océanographie

Fig. 2
Bathymetry map of Dease Strait and the Cambridge Bay area. The blue box indicates the area over which the model output is averaged for comparison with
data collected by ONC’s cabled underwater observatory, whose location is indicated by the star (at Cambridge Bay). The red dots labelled “R1” to “R5”
indicate the locations of CTD measurements on the R/V Martin Bergmann, while the blue line that connects these locations indicates the R-transect.
Fig. 3
Time series of (a) ice thickness, (b) temperature, (c) salinity, and (d) density from November 2014 to November 2018 in the Cambridge Bay area at a depth
of 8 m. Blue curves show data from the ANHA12 model, and red curves show data from ONC measurements. In (a), only the ice draft is reported by ONC.
The ice thickness shown in the plot is estimated as 1.11 times of the ice draft, assuming that the density of ice is roughly 90% of the density of seawater.
Effects of Seasonal Ice Coverage on the Kitikmeot Sea / 219
ATMOSPHERE-OCEAN 59 (4–5) 2021, 214–232
https://doi.org/10.1080/07055900.2021.1965531
Canadian Meteorological and Oceanographic Society

period in each year, leading to greater seasonal variations in
its physical oceanographic conditions. For example, the for-
mation and melting of sea ice plays a dominant role in the sea-
sonality of the surface freshwater processes (Hu et al., 2019).
Our simulation results suggest that, on average, ice concen-
tration in the Kitikmeot Sea is at or about 100% from Novem-
ber to June each year, with the ice thickness reaching its
maximum in May, although spatial and interannual variations
exist. For the purposes of our analysis, we shall consider
November to June as the ice-covered season and July to
October as the ice-free season.
Figure 7 shows that, when averaged over ice-covered
seasons from 2011 to 2018, ice thickness is over 2 m in
M’Clintock Channel and reduces gradually from QMG to
CG. In Amundsen Gulf, the average ice thickness is less
than 1 m. The decrease in ice thickness from M’Clintock
Channel to Amundsen Gulf is consistent with previous
studies of sea ice in the CAA (e.g., Haas and Howell, 2015;
Michel et al., 2015), which suggested that multi-year ice
(MYI) of signiﬁcant concentration and thickness exists in
the northern CAA and M’Clintock Channel, while Amundsen
Gulf and CG is dominated by ﬁrst-year ice (FYI) of less con-
centration and thickness. Hu et al. (2018) showed that the net
production of sea ice in the CAA is due to both thermodyn-
amic (e.g., cold temperature) and dynamic (e.g., local advec-
tion) processes, though their contribution can be either
positive or negative, and that when FYI dominates, thermo-
dynamic processes play a more important role in determining
the ice thickness. Along the Alaska coast, the seasonal ice
melt is mainly triggered by an inﬂow of warm Paciﬁc water
(Shimada et al., 2006; Woodgate et al., 2006). Because
exchange of water (and hence the heat content) between the
Kitikmeot Sea and the rest of the Arctic Ocean is relatively
limited due to the existence of the bounding sills, melt in
this region is likely driven by local thermodynamic processes.
Two sites, located at the deepest points of QMG (≏125 m)
and CG (≏325 m), respectively, were selected to further study
the ice conditions and their seasonal variations. These
locations are indicated by the red dots in Fig. 7. A comparison
of the ice conditions at these two locations is shown in Fig. 8.
It is clear from this ﬁgure that QMG has a longer ice-covered
period in general and that ice thickness in QMG is larger than
Fig. 4
(a)–(d) Salinity measured from the R/V Martin Bergmann across the R-transect as shown in Fig. 2 in early August of 2015–2018. (e)–(h) Corresponding
salinity data from output of the ANHA12 model. The model data are averaged over a ﬁve-day period. Black curves indicate the approximate location of the
ocean bottom. Note that the observational data for 2018 are incomplete for technical reasons.
220 / Chengzhu Xu et al.
ATMOSPHERE-OCEAN 59 (4–5) 2021, 214–232
https://doi.org/10.1080/07055900.2021.1965531
La Société canadienne de météorologie et d’océanographie

that in CG, especially during 2014–2015. The ﬂuctuation of
the ice conditions in QMG could be one of the reasons that
the model results are different from the measurements by
ONC as illustrated in Fig. 3. The ONC observatory is
located very close to the shore, thus is less likely to be affected
by the larger-scale ice dynamics in QMG and M’Clintock
Channel. However, by analyzing data from 1980 to 2012,
Michel et al. (2015) showed that there is a 20–40% chance
of MYI being present in QMG, which is much more frequent
than our model estimation from 2011 to 2018 suggested. This
is most likely because, in recent years, there has been a signiﬁ-
cant decrease in the summer sea-ice extent and MYI extent in
the southern CAA, especially in M’Clintock Channel (Howell
et al., 2013; Michel et al., 2015).
For the two study sites discussed above, ice thickness
changes due to the thermodynamic and dynamic processes,
as well as ice thickness changes from both processes com-
bined, are shown in Fig. 9. It is clear from the ﬁgure that
Fig. 5
As in Fig. 4 but for temperature.
Fig. 6
Time series of ice volume within the Kitikmeot Sea from 2011 to 2018.
Effects of Seasonal Ice Coverage on the Kitikmeot Sea / 221
ATMOSPHERE-OCEAN 59 (4–5) 2021, 214–232
https://doi.org/10.1080/07055900.2021.1965531
Canadian Meteorological and Oceanographic Society

during the freezing season, which starts in December at both
locations and lasts until late April in CG and mid-May in
QMG, changes in ice thickness due to dynamic processes
are approximately zero, and thermodynamic processes play
a dominant role in determining ice thickness in both locations.
This means that most sea ice during this period is landfast ice.
During the melting period, which is from May to early July in
CG and to late August in QMG, thermodynamic processes
contribute to the majority of ice reduction. In fact, ice move-
ment from M’Clintock Channel to QMG through VS is
evident because dynamic processes lead to an increase in
the ice thickness during this period, as shown in Fig 9b. In
CG, on the other hand, Fig 9a shows that dynamic processes
lead to a decrease in ice thickness during the same period,
implying the existence of a net outﬂow of sea ice. After the
melting period, there is a period during which changes in
ice thickness are nearly zero in CG from mid-July to late Sep-
tember, whereas in QMG such a period of time does not exist.
Finally, ice thickness starts to increase due to dynamic pro-
cesses in late September in CG and early September in
QMG, while the local freeze-up due to thermodynamic pro-
cesses begins in late October in CG and early October in
QMG.
While sea ice that grows and decays in situ (i.e., due to ther-
modynamic processes) does not change the overall freshwater
content in this region, the transport of sea ice due to dynamic
processes is important for freshwater input. To quantify the
net inﬂow of sea ice into the Kitikmeot Sea, we calculated
the volume ﬂux of sea ice through both DUS and VS. The
results are shown in Fig. 10, where positive values represent
inﬂow and negative values represent outﬂow. The ﬁgure
shows that, although landfast ice exists between November
and May, signiﬁcant ice movement occurs from May to
November, with peak volume reaching 200 km3 yr−1 at VS.
When a time average is taken from 2011 to 2018, the net
inﬂow of sea ice is 24.61 km3 yr−1 through VS and
Fig. 7
Average ice thickness in the Kitikmeot Sea from November to June (averaged from 2011 to 2018). Red dots indicate locations within Coronation Gulf and
Queen Maud Gulf at which the time series in Fig. 8 are produced.
Fig. 8
Time series of (a) ice concentration and (b) ice thickness in Coronation Gulf (red) and Queen Maud Gulf (blue) from 2011 to 2018. The particular locations
for which the time series are produced are indicated by the red dots in Fig. 7.
222 / Chengzhu Xu et al.
ATMOSPHERE-OCEAN 59 (4–5) 2021, 214–232
https://doi.org/10.1080/07055900.2021.1965531
La Société canadienne de météorologie et d’océanographie

−10.83 km3 yr−1 through DUS. Given the positive net inﬂow
though VS and negative net inﬂow through DUS, it is clear
that the direction of ice movement is generally westward
(i.e., from VS to DUS). Combining the net inﬂow at these
two open boundaries, the total inﬂow of sea ice into the Kitik-
meot Sea is approximately 14 km3 yr−1, equivalent to
approximately one-sixth of the maximum volume of sea ice
that remains at the end of the freezing season, and one-third
of the freshwater input from river runoff (≏41 km3 yr−1; Wil-
liams et al., 2018). Therefore, the fact that there is a net inﬂow
of sea ice that melts locally each summer suggests that the
movement of sea ice is a signiﬁcant freshwater pathway and
has an effect on surface salinity distributions similar to
runoff from land.
b Stratiﬁcation
Stratiﬁcation of the Kitikmeot Sea is a unique characteristic
because of the large amount of freshwater input and the exist-
ence of the shallow bounding sills. Most notably, the salinity
of the Kitikmeot Sea is much lower than that in most of the
Arctic Ocean. The freshwater storage of the Kitikmeot Sea
is calculated using the method described in Section 2 and
shown as a time series in Fig. 11. Although interannual varia-
bility exists, the difference between maximum and minimum
freshwater storage each year is 100–150 km3 in the ocean
only and slightly over 50 km3 when combined with fresh
water in sea ice. The sources of freshwater input include
melted ice, runoff from land, and net precipitation. The con-
tribution from the latter is relatively minor given the short
Fig. 9
Seasonal cycle (averaged from 2011 to 2018) of overall ice thickness changes (blue), and ice thickness changes due to thermodynamic (red) and dynamic
(black) processes at (a) Coronation Gulf and (b) Queen Maud Gulf. The particular locations for which the time series are produced are indicated by the red
dots in Fig. 7.
Fig. 10
Seasonal cycle (averaged from 2011 to 2018) of net inﬂow of sea ice through Dolphin and Union Strait (blue) and Victoria Strait (orange).
Effects of Seasonal Ice Coverage on the Kitikmeot Sea / 223
ATMOSPHERE-OCEAN 59 (4–5) 2021, 214–232
https://doi.org/10.1080/07055900.2021.1965531
Canadian Meteorological and Oceanographic Society

duration of the ice-free season. On the other hand, the actual
amount of freshwater input from melted ice could be more
than the maximum volume of sea ice existing at the end of
each freezing season because a net inﬂow of sea ice into the
Kitikmeot Sea occurs during the melting season, as discussed
in the previous section. While a year-to-year increase in fresh-
water content can also be seen in Fig. 11, it is not clear
whether such interannual variability is a long-term trend
given the short simulation period. The interannual variability
is not the focus of this paper and should be discussed in future
studies when simulations are performed over a much longer
time span.
The freshwater and heat contents of the Kitikmeot Sea,
calculated using the method described in Section 2.c and
averaged from 2011 to 2018, are shown in Fig. 12. To be
consistent with literature on the freshwater content of the
Arctic Ocean (e.g., McPhee et al., 2009), the reference
salinity for calculating the freshwater content is set to
Sref = 34.8. This value is larger than the maximum salinity
of the Kitikmeot Sea, so that the entire water column is
included in the integration. For this reason, CG has much
more freshwater content than QMG because it is much
deeper in general. The average depth of freshwater content
in CG is approximately 30 m, while the maximum depth
can reach as much as 39 m. This value almost doubles the
freshwater content per unit area in the Beaufort Gyre
(Proshutinsky et al., 2019), which is signiﬁcantly inﬂuenced
by Paciﬁc water. Although Paciﬁc water is the primary
source of fresh water entering the Arctic Ocean (Haine
et al., 2015; Woodgate et al., 2010), our results suggest
that the Paciﬁc water is not nearly as fresh as water in the
Kitikmeot Sea. In fact, any inﬂow of water of Paciﬁc
origin through either DUS or VS will decrease the fresh-
water content of the Kitikmeot Sea by displacing the
fresher water within this region.
Figure 12b shows that the surface water in CG is warmer
than in QMG in general. This is most likely due to the
longer open water season in CG (Fig. 8a) and the net
inﬂow of sea ice into QMG from M’Clintock Channel
(Figs 9 and 10). Note that the heat content in most areas
of QMG is negative because when the time average is
taken, the averaged temperature is below the reference temp-
erature of 0°C.
Three sites, located at the deepest points of QMG and CG,
respectively, and in DS between Cambridge Bay and Kent
Peninsula (R3 in Fig. 2), were selected to further study the
seasonal variation of salinity and temperature. These locations
are indicated by the red/cyan dots in Fig. 12. The Hovmöller
plots of salinity and temperature at these locations are shown
in Figs 13 and 14, respectively, while several quantitative
measurements are given in Table 2. Note that there is a
signiﬁcant difference in terms of the bottom depth at these
three locations (DS: ≏75 m; QMG: ≏125 m; CG: ≏325 m),
as indicated by the different scales of the y-axes in these plots.
Both Figs 13 and 14 show that the surface layer at all
locations exhibits signiﬁcant seasonal variation. Because of
ice melting and river discharge, the minimum salinity in DS
and QMG can reach as low as approximately 15 in the
Fig. 11
Time series of freshwater storage in the Kitikmeot Sea from 2011 to 2018. Blue: freshwater storage in the ocean only; red: freshwater storage in the ocean
and sea ice combined. The freshwater storage in the ocean is calculated based on the reference salinity Sref = 34.8, while the freshwater storage in the ice
is approximated as 0.9 times the ice volume.
TABLE 2.
Model estimation of minimum, maximum, and average salinity,
temperature, and potential density between 2011 and 2018 at
locations indicated by the red/cyan dots in Fig. 12.
Location
Dease
Strait
Queen Maud
Gulf
Coronation
Gulf
Minimum salinity
14.5
15.2
19.9
Maximum salinity
31.0
31.5
33.0
Average salinity, upper 40 m
27.4
27.1
27.4
Average salinity, 40–150 m
29.3
30.2
29.6
Average salinity, below
150 m
N/A
N/A
32.5
Minimum temperature (°C)
−1.7
−1.6
−1.9
Maximum temperature (°C)
8.0
6.3
10.1
Average temperature, upper
40 m (°C)
−0.4
−0.7
0.3
Average temperature, 40–
150 m (°C)
−0.1
−0.2
0.5
Average temperature, below
150 m (°C)
N/A
N/A
−0.8
Minimum density (kg m−3)
1011.54
1012.11
1015.90
Maximum density (kg m−3)
1024.88
1025.27
1026.50
Average density, upper 40 m
(kg m−3)
1022.00
1021.77
1021.90
Average density, 40–150 m
(kg m−3)
1023.55
1024.27
1023.76
Average density, below
150 m (kg m−3)
N/A
N/A
1026.10
224 / Chengzhu Xu et al.
ATMOSPHERE-OCEAN 59 (4–5) 2021, 214–232
https://doi.org/10.1080/07055900.2021.1965531
La Société canadienne de météorologie et d’océanographie

summer, and the minimum salinity in CG can also drop to
below 20 in certain years. The temperature of the surface
layer varies from approximately −2°C in the winter to
approximately 6°C, 8°C, and 10°C in QMG, DS, and CG,
respectively, in the summer. The seasonal variation is also
evident in terms of the thickness of the surface layer. If we
deﬁne the bottom of the surface layer by the 29 isopycnal,
as indicated by the black contours in Fig. 13, then it is clear
that the layer thickness varies from approximately 20 m by
the end of the ice-covered season to over about 60 m by the
end of the ice-free season. In late spring/early summer each
year when sea ice and river ice melts, fresh water enters the
Kitikmeot Sea, decreasing the surface salinity. In the mean-
time, the surface temperature also increases as a result of
direct solar radiation. Because of direct wind-driven mixing,
the surface layer also deepens (Lincoln et al., 2016; Rainville
et al., 2011). The entire water column of DS can be well
mixed by the end of the ice-free season, though this is not
the case in CG or QMG due to the larger depth. The deepening
of the surface layer stops in early winter (November) when
sea ice forms again, which acts as a rigid lid that prevents
direct wind-driven mixing of the surface layer, and the pycno-
cline shoals gradually due to the outﬂow of surface fresh
water through DUS and VS.
The layer immediately below the surface layer extends to
approximately 150 m depth, which is below the bottom depth
of DS and QMG. This layer has an average salinity of
approximately 30 and an average temperature of 0°C and
does not show signiﬁcant seasonal variation except for its
upper boundary. In CG, there is also a bottom layer, which
has an average salinity of approximately 32.5 and an
average temperature of −0.8°C, that is not inﬂuenced by
the seasonal cycle of the surface layer. Mixing between the
bottom and middle layers in CG is likely to be limited
because the pycnocline appears quiescent throughout the
entire study period. The bottom waters could be from over-
ﬂow of the saltier and denser water through DUS and VS
and are likely to consist of water of Paciﬁc origin, given
the close proximity of the Kitikmeot Sea to the Alaska
coast and channels in the northern CAA that are a major
pathway for Paciﬁc water transport (Dmitrenko et al.,
2018; Hu et al., 2019). Hence, the bottom water is likely to
have a relatively higher nutrient concentration, and thus is
potentially important for biogeochemical processes. The
exact origin of the bottom water and the processes of deep
water renewal and diapycnal mixing in the Kitikmeot Sea
are beyond the scope of this paper but are a potential topic
for future study.
Fig. 12
(a) Freshwater content above the 34.8 halocline and (b) heat content per unit area in the upper 40 m, averaged from 2011 to 2018. Red/cyan dots indicate
locations within DS, QMG, and CG at which Hovmöller plots in Figs 13 and 14 are produced.
Effects of Seasonal Ice Coverage on the Kitikmeot Sea / 225
ATMOSPHERE-OCEAN 59 (4–5) 2021, 214–232
https://doi.org/10.1080/07055900.2021.1965531
Canadian Meteorological and Oceanographic Society

Fig. 13
Hovmöller plots of salinity at (a) Dease Strait, (b) Queen Maud Gulf, and (c) Coronation Gulf as a function of time and depth from 2011 to 2018. The
black contours indicate the depth of the 29 halocline. The particular locations at which the Hovmöller plots are produced are indicated by the red/cyan
dots in Fig. 12. Note the different scales on the y-axes (depth) for each plot (location). The bottoms of Dease Strait (DS) and Queen Maud Gulf (QMG) are
indicated by the dashed lines in (b) and (c).
Fig. 14
As in Fig. 13 but for temperature.
226 / Chengzhu Xu et al.
ATMOSPHERE-OCEAN 59 (4–5) 2021, 214–232
https://doi.org/10.1080/07055900.2021.1965531
La Société canadienne de météorologie et d’océanographie

The Hovmöller plots for density proﬁles are not shown
because the density in the Kitikmeot Sea is primarily deter-
mined by the salinity, and the density plots appear almost
identical to the salinity plots. Nevertheless, some quantitative
measurements are given in Table 2. In particular, the table
suggests that the minimum density can reach approximately
1011 kg m−3, while the average density of the middle layer
is approximately 1022 kg m−3, implying the existence of
strong stratiﬁcation in the Kitikmeot Sea, at least in the
summer.
c Circulation
The ocean circulation of the Kitikmeot Sea is signiﬁcantly
inﬂuenced by seasonal ice coverage (and hence wind stress
in the ice-free season) and the Earth’s rotation. The seasonal
variation of ocean currents can be seen in Fig. 15. Only the
zonal component of the velocity ﬁeld is shown because the
Kitikmeot Sea is an east–west waterway, which means that
the zonal component is dominant. Figure 15 clearly shows
that ocean current in the ice-free season is much stronger
than that in the ice-covered season. The reason for this is
that without ice coverage, momentum input from wind
stress can reach the ocean surface and affect the ocean circu-
lation directly. Compared with water bodies at a lower lati-
tude, the Coriolis effect plays a more important role in the
circulation of the Kitikmeot Sea and channels in the CAA
in general. Nurser and Bacon (2014) showed that the
Rossby radius of deformation of a typical channel within
the CAA is approximately 6 km. The Kitikmeot Sea has a
width of 20–50 km (the narrowest cross-section at DS is
approximately 20 km), much larger than the deformation
radius. Because of the Coriolis effect, ﬂows to the east are
along the south shore, and ﬂows to the west are along the
north shore (see Fig. 6 of McLaughlin et al., 2004, for a
detailed description of this mechanism). This ﬂow pattern is
not unique to the Kitikmeot Sea but is also observed in
other channels within the CAA (see, for example, Fig. 3 of
Hughes et al., 2017).
Unlike channels in the northern CAA in which the mean
ﬂow is generally southward and/or eastward, in the Kitikmeot
Sea the ﬂow direction is not consistent but exhibits signiﬁcant
variation related primarily to the wind stress, especially during
the ice-free season when the ﬂow is driven directly by the wind
stress. To illustrate the relationship, a comparison of surface
velocity from model output and wind stress from the CGRF
dataset in August of selected years is shown in Fig. 16. The
ﬁgure shows that northwest winds are dominant in 2012 and
2018 while southeast winds are dominant in 2015. As a
result, the surface current is also eastward in 2012 and 2018
and westward in 2015. Comparing 2012 with 2018, the
Fig. 15
Time- and depth-averaged zonal velocity of the Kitikmeot Sea from 2011 to 2018. (a) Ice-covered season: November–June. (b) Ice-free season: July–
October. Positive velocity (red) is eastward, and negative velocity (blue) is westward.
Effects of Seasonal Ice Coverage on the Kitikmeot Sea / 227
ATMOSPHERE-OCEAN 59 (4–5) 2021, 214–232
https://doi.org/10.1080/07055900.2021.1965531
Canadian Meteorological and Oceanographic Society

ﬁgure shows that when the wind speed is faster, the surface
current is also stronger. The surface current is also inﬂuenced
by topographical features. As evident in Figs 16a and 16c,
under northwest winds, a strong current exists to the south of
Finlayson Islands along the shore of Kent Peninsula. On the
other hand, Fig. 16b shows that under southeast winds, ﬂow
is stronger along the north shore near Cambridge Bay. This
is consistent with the ﬂow pattern shown in Fig. 15, which
suggests that the time-averaged current is eastward along the
south shore of DS and westward along the north shore. When
the wind direction is mainly northward or southward, the
surface current does not have a preferred direction. Figure 16
also shows that the Finlayson Islands in the middle of DS
block and alter the ﬂow to a certain degree.
Fig. 16
(a), (c), and (e) Time-averaged surface velocity near Cambridge Bay for August 2012, 2015, and 2018. Pseudo-colour map and arrows show the speed
and direction of the surface current. (b), (d), and (f) Distribution of hourly wind data from the CGRF dataset for August 2012, 2015, and 2018. The data
are averaged over the region of 104°–108°W and 68.25°–69.45°N. Blue, green, and red in each bin of the wind rose diagram indicate, wind speeds of 0–
5 m s−1, 5–10 m s−1, and over 10 m s−1, respectively. The length of each of the bins indicates the percentage of the hourly wind data from the correspond-
ing direction.
228 / Chengzhu Xu et al.
ATMOSPHERE-OCEAN 59 (4–5) 2021, 214–232
https://doi.org/10.1080/07055900.2021.1965531
La Société canadienne de météorologie et d’océanographie

Before concluding this section, we would like to note that
tidal forcing, although not included in the present model, is
also expected to have a non-negligible inﬂuence on the circu-
lation of the Kitikmeot Sea, especially on a semi-diurnal to
diurnal time scale. Because tidal currents are strong near the
bounding sills at DUS and VS, they can signiﬁcantly affect
the inﬂow and outﬂow through these open boundaries, as
well as the ice movement during the melting season. Future
studies of the mixing in the Kitikmeot Sea should include
explicit tidal forcing.
5 Discussion
The physical oceanographic conditions of the Kitikmeot Sea
are different from channels in the northern CAA primarily
because of the existence of a substantial ice-free period
each year. Such ice conditions have several signiﬁcant impli-
cations for both stratiﬁcation and ocean circulation. During
the ice-free season, external forcings such as ice melt, river
discharge, and solar radiation can act directly on the sea
surface, leading to freshening and warming of the surface
water. Our simulation results suggest that the minimum sal-
inity in the Kitikmeot Sea can reach as low as 15 during the
peak runoff season, while the minimum density can be
close to 1010 kg m−3. In contrast, the average salinity and
density
of
the
bottom
layer
at
CG
are
32.5
and
1026 kg m−3, respectively, and do not show evidence of sea-
sonal variation. Such a large salinity and density gradient
implies the existence of strong stratiﬁcation in the Kitikmeot
Sea, which signiﬁcantly restricts mixing between the surface
and bottom water.
The annual variation of freshwater storage in the Kitikmeot
Sea, when combined from both the ocean and the sea ice, is
slightly greater than 50 km3. The freshwater input during
the melting season is a result of the net inﬂow of sea ice,
runoff from land, and net precipitation and is approximately
balanced by the net outﬂow of fresh water during the freezing
season. Quantitatively, the runoff contributes approximately
41 km3 of fresh water each year (Williams et al., 2018) and
is a major source of freshwater input, while the net inﬂow
of
sea
ice
through
DUS
and
VS
is
approximately
14 km3 yr−1. Being approximately one-third of the total
volume of runoff, the net inﬂow of sea ice is also a non-neg-
ligible source of the freshwater input. On the other hand, the
contribution from net precipitation is relatively minor, given
that the average volume of freshwater input from the other
two sources adds up to over 50 km3.
As a result of differences in dynamic ice transport across
the region, QMG is characterized by thicker ice and lower
summer surface salinity and temperature. Generally, ice
melt and low surface temperatures drive low surface
pCO2, while warmer surface temperatures drive higher
pCO2. This spatial variation helps explain the biogeochem-
ical observations of Ahmed et al. (2019), who found that
QMG typically acts as a sink for atmospheric CO2 in the
summer, while CG typically acts as a source of CO2 to
the atmosphere.
In the Arctic Ocean, inﬂow of water from the Paciﬁc Ocean
has been recognized as a dominant source of fresh water, heat,
and nutrients (Haine et al., 2015; Woodgate et al., 2010). Pre-
vious studies (e.g., Dmitrenko et al., 2018; Hu et al., 2019)
showed that Paciﬁc water mainly follows the Transpolar
Drift and is a major source of freshwater content in the Beau-
fort Gyre. Nevertheless, our simulation results suggest that the
freshwater content per unit area in CG is larger than that in the
Beaufort Gyre in general because of the signiﬁcant amount of
ice melt and runoff from land relative to the small size of the
Kitikmeot Sea. Our results also suggest that only the bottom
water in CG has an average salinity that is similar to the sal-
inity of the Paciﬁc water, which varies seasonally from 31.9 to
33 (Woodgate, 2018). Thus, any Paciﬁc water ﬂowing into the
Kitikmeot Sea is likely to sink to the bottom immediately,
given its relatively higher density than the surface water of
the Kitikmeot Sea. As a result, any nutrients brought into
this region by Paciﬁc water are also likely to concentrate in
the bottom layer because of the strong stratiﬁcation and
limited vertical mixing. This could be a potentially important
reason why the Kitikmeot Sea has low primary production
and biological productivity (Back et al., 2021; Bouchard
et al., 2018).
Another effect of the seasonal ice coverage is that, in the
ice-free season, wind stress can force the ocean circulation
directly. The direct momentum input from wind stress into
the ocean leads to both wind-driven mixing and wind-
driven circulation. The wind-driven mixing deepens the
surface layer throughout the ice-free season. Although the
mixing is not strong enough to reach the more nutrient-rich
bottom layer, it still allows full mixing at locations where
shallow sills exist such as in DS, DUS, and VS. As a result,
the entire water column in these straits are well mixed. There-
fore, direct wind-driven mixing can also lead to enhanced ver-
tical
mixing
and
potentially
increase
the
biological
productivity in a similar way to the effect of tidal forcing
(Dalman et al., 2019). To determine the relative importance
of each of these mechanisms, a budget analysis should also
include tidal mixing. It could be performed in future studies
when tidal forcing is included in the model.
The wind-driven circulation in the ice-free season leads to a
strong along-shore current whose speed can reach 0.1 m s−1.
This current speed is almost an order of magnitude larger
than that during the ice-covered season, which is only
approximately 0.01 m s−1. The ﬂow direction in the Kitik-
meot Sea is not consistent and is heavily inﬂuenced by the
wind direction. In contrast, a strong southward/eastward
mean ﬂow exists in the northern CAA (McLaughlin et al.,
2004), and the typical speed is on the order of 0.1 cm s−1 in
most of the channels (Hughes et al., 2017). This implies
that the ocean current in the Kitikmeot Sea is generally
slow except when there are strong winds in the ice-free
season. This provides some support for the hypothesis that
the near-absence of Arctic cod in the Kitikmeot Sea is
Effects of Seasonal Ice Coverage on the Kitikmeot Sea / 229
ATMOSPHERE-OCEAN 59 (4–5) 2021, 214–232
https://doi.org/10.1080/07055900.2021.1965531
Canadian Meteorological and Oceanographic Society

partially caused by their larvae not being easily dispersed by
the slow current (Bouchard et al., 2018).
Although the present work focuses primarily on the seaso-
nal variation of the physical oceanographic conditions of the
Kitikmeot
Sea,
interannual
variation
is
also
evident,
especially in the freshwater storage shown in Fig. 11. The
increase in freshwater storage over the simulation period
could be a consequence of climate change that leads to the
freshening of the Arctic Ocean in general (McPhee et al.,
2009). However, it might also be a short-term trend, or even
a model artifact, possibly a result of the lack of tidal forcing
and hence inaccurate inﬂow and outﬂow conditions. Simu-
lations of a much longer time span are needed to determine
the existence of any systematic and long-term impact of
climate change on this region. Nevertheless, if the increase
in freshwater storage and freshening of the surface layer is
a result of climate change, then it is reasonable to assume
that the stratiﬁcation of the Kitikmeot Sea could be even
stronger in the future, such that the mixing efﬁciency and
hence primary production would be further reduced. Further
studies are needed to determine the implication of changing
oceanographic conditions on the ecosystem and biogeochem-
istry in the Kitikmeot Sea.
6 Conclusions
In conclusion, the seasonal variation of ice coverage leads to a
strong seasonal variation of the physical oceanographic con-
ditions, including both stratiﬁcation and circulation, of the
Kitikmeot Sea. The seasonal variation of the stratiﬁcation is
characterized by the freshening of the surface layer through
the melting of sea ice and runoff from land in the ice-free
season, which signiﬁcantly increases the strength of stratiﬁca-
tion and limits vertical mixing. The circulation, on the other
hand, is primarily driven by wind stress in the ice-free
season, which leads to deepening of the surface layer and
an increase in current speed, both of which increase the
mixing efﬁciency, at least for the surface layer. Nevertheless,
given the short duration of the ice-free season and the large
amount of freshwater input, the increase in mixing due to
wind-driven circulation is limited to only the surface layer
and locations where shallow sills exist. For deep basins,
especially in CG, the bottom layer does not show evidence
of any signiﬁcant seasonal variation, and diapycnal mixing
is limited throughout the year.
The model used in the present work has some limitations.
For example, it has limited resolution, and a model with
higher resolution would allow for more accurate and detailed
analyses for future studies. For this reason, a higher-resolution
model is under development, with the target resolution being
1/60° in the horizontal directions. Another major limitation of
the present model is the lack of tidal forcing, which is
expected to play an important role in the inﬂow and outﬂow
through the bounding sills at the open boundaries. With
more accurate inﬂow and outﬂow data, we will be able to
better quantify the budget of freshwater storage and the
process of deep water renewal, both of which are essential
for a better understanding of the mixing in this region, as
well as the biological productivity and the entire ecosystem.
Acknowledgements
A portion of the data used in this work was provided by Ocean
Networks Canada. We thank the captain and crew of the R/V
Martin Bergmann for their assistance collecting the CTD vali-
dation data. We thank Mike Dempsey at Fisheries and Oceans
Canada for annually calibrating the CTD rosette. Logistical
support was provided by Polar Knowledge Canada and the
Arctic Research Foundation. Shawn Marriott and Francis
Emingak assisted with ﬁeld data collection. Special thanks
to C.J. Mundy for his role in initiating and continuing the
R/V Martin Bergmann cruises. We thank the NEMO develop-
ment team as well as the DRAKKAR group for providing the
model code and continuous guidance. We express our thanks
to
Westgrid
and
Compute
Canada
(http://www.
computecanada.ca) for the computational resources to carry
out our numerical simulations as well as archiving of the
experiments. We thank Charlene Feucher and Clark Pennelly
for helping with the access and analysis of the ANHA12
model. We would also like to thank the two anonymous refer-
ees for their constructive comments and concrete suggestions
that led to signiﬁcant improvements of this work.
Disclosure statement
No potential conﬂict of interest was reported by the author(s).
Funding
Financial support was provided by the Marine Environmental
Observation Prediction and Response (MEOPAR) network,
the Natural Sciences and Engineering Research Council of
Canada (NSERC), Irving Shipbuilding, and the ArcticNet
network. The involvement of P.G.M. in this work was sup-
ported by an NSERC Climate Change and Atmospheric
Research Grant [grant number RGPCC 433898] as well as
an NSERC Discovery Grant [grant number RGPIN 04357].
Q.Z. was supported, in part, by an NSERC Discovery Grant
[grant number RGPIN-2018-04329].
ORCID
Qi Zhou
http://orcid.org/0000-0002-7731-0360
References
Ahmed, M., Else, B. G. T., Burgers, T. M., & Papakyriakou, T. (2019).
Variability of surface water pCO2 in the Canadian Arctic Archipelago
from 2010 to 2016. Journal of Geophysical Research: Oceans, 124(3),
1876–1896. https://doi.org/10.1029/2018JC014639
230 / Chengzhu Xu et al.
ATMOSPHERE-OCEAN 59 (4–5) 2021, 214–232
https://doi.org/10.1080/07055900.2021.1965531
La Société canadienne de météorologie et d’océanographie

Amante, C., & Eakins, B. W. (2009). ETOPO1 1 arc-minute global relief
model: Procedures, data sources and analysis (NOAA technical memor-
andum NESDIS NGDC-24). National Geophysical Data Center, NOAA.
Back, D.-Y., Ha, S.-Y., Else, B., Hanson, M., Jones, S. F., Shin, K.-H.,
Tatarek, A., Wiktor, J. M., Cicek, N., Alam, S., & Mundy, C. J. (2021).
On the impact of wastewater efﬂuent on phytoplankton in the Arctic
coastal zone: A case study in the Kitikmeot Sea of the Canadian Arctic.
Science of The Total Environment, 764, 143861. https://doi.org/10.1016/
j.scitotenv.2020.143861
Bamber, J., van den Broeke, M., Ettema, J., Lenaerts, J., & Rignot, E. (2012).
Recent large increases in freshwater ﬂuxes from Greenland into the North
Atlantic. Geophysical Research Letters, 39(19), L19501. https://doi.org/
10.1029/2012GL052552
Bernard, B., Madec, G., Penduff, T., Molines, J.-M., Treguier, A.-M., Le
Sommer, J., Beckmann, A., Biastoch, A., Böning, C., Dengg, J., Derval,
C., Durand, E., Gulev, S., Remy, E., Talandier, C., Theetten, S.,
Maltrud, M., McClean, J., & De Cuevas, B. (2006). Impact of partial
steps and momentum advection schemes in a global ocean circulation
model at eddy-permitting resolution. Ocean Dynamics, 56(5), 543–567.
https://doi.org/10.1007/s10236-006-0082-1
Bouchard, C., Geoffroy, M., LeBlanc, M., & Fortier, L. (2018). Larval and
adult ﬁsh assemblages along the Northwest Passage: The shallow
Kitikmeot and the ice-covered Parry Channel as potential barriers to disper-
sal. Arctic Science, 4(4), 781–793. https://doi.org/10.1139/as-2018-0003
Dai, A., Qian, T., Trenberth, K. E., & Milliman, J. D. (2009). Changes in con-
tinental freshwater discharge from 1948 to 2004. Journal of Climate, 22
(10), 2773–2792. https://doi.org/10.1175/2008JCLI2592.1
Dalman, L., Else, B., Barber, D., Carmack, E., Williams, W., Campbell, K.,
Duke, P. J., Kirillov, S., & Mundy, C. (2019). Enhanced bottom-ice algal
biomass across a tidal strait in the Kitikmeot Sea of the Canadian Arctic.
Elementa: Science of the Anthropocene, 7, 22. https://doi.org/10.1525/
elementa.361
Dmitrenko, I. A., Kirillov, S. A., Myers, P. G., Forest, A., Tremblay, B.,
Lukovich, J. V., Gratton, Y., Rysgaard, S., & Barber, D. G. (2018).
Wind-forced depth-dependent currents over the eastern Beaufort Sea con-
tinental slope: Implications for Paciﬁc water transport. Elementa: Science
of the Anthropocene, 6, 66. https://doi.org/10.1525/elementa.321
DRAKKAR Group. (2007). Eddy-permitting ocean circulation hindcasts of
past decades. CLIVAR Exchanges, 12(3), 8–10.
Duke, P., Else, B., Jones, S., Marriott, S., Ahmed, M., Nandan, V.,
Butterworth, B., Gonski, S. F., Dewey, R., Sastri, A., Miller, L. A.,
Simpson, K. G., & Thomas, H. (2021). Seasonal marine carbon system
processes in an Arctic coastal landfast sea ice environment observed
with an innovative underwater sensor platform. Elementa: Science of the
Anthropocene, (Accepted).
Falardeau-Côté, M. (2020). The Arctic Ocean under multiple pressures:
Linking impacts on marine ecosystem processes, ecosystem services, and
human
well-being
[Unpublished
doctoral
dissertation].
McGill
University. https://escholorrship.mcgill.ca/concern/theses/fq978033q
Fichefet, T., & Maqueda, M. A. M. (1997). Sensitivity of a global sea ice
model to the treatment of ice thermodynamics and dynamics. Journal of
Geophysical Research: Oceans, 102(C6), 12609–12646. https://doi.org/
10.1029/97JC00480
Haas, C., & Howell, S. E. L. (2015). Ice thickness in the Northwest Passage.
Geophysical Research Letters, 42(18), 7673–7680. https://doi.org/10.
1002/2015GL065704
Haine, T. W. N., Curry, B., Gerdes, R., Hansen, E., Karcher, M., Lee, C.,
Rudels, B., Spreen, G., de Steur, L., Stewart, K. D., & Woodgate, R.
(2015). Arctic freshwater export: Status, mechanisms, and prospects.
Global and Planetary Change, 125, 13–35. https://doi.org/10.1016/j.
gloplacha.2014.11.013
Harris, L. N., Yurkowski, D. J., Gilbert, M. J. H., Else, B. G. T., Duke, P. J.,
Ahmed, M. M. M., Tallman, R. F., Fisk, A. T., & Moore, J.-S. (2020).
Depth and temperature preference of anadromous Arctic char Salvelinus
alpinus in the Kitikmeot Sea, a shallow and low-salinity area of the
Canadian Arctic. Marine Ecology Progress Series, 634, 175–197. https://
doi.org/10.3354/meps13195
Howell, S. E. L., Derksen, C., Pizzolato, L., & Brady, M. (2015). Multiyear
ice replenishment in the Canadian Arctic Archipelago: 1997–2013.
Journal of Geophysical Research: Oceans, 120(3), 1623–1637. https://
doi.org/10.1002/2015JC010696
Howell, S. E. L., Wohlleben, T., Dabboor, M., Derksen, C., Komarov, A., &
Pizzolato, L. (2013). Recent changes in the exchange of sea ice between
the Arctic Ocean and the Canadian Arctic Archipelago. Journal of
Geophysical Research: Oceans, 118(7), 3595–3607. https://doi.org/10.
1002/jgrc.20265
Hu, X., Myers, P. G., & Lu, Y. (2019). Paciﬁc water pathway in the Arctic
Ocean and Beaufort Gyre in two simulations with different horizontal res-
olutions. Journal of Geophysical Research: Oceans, 124(8), 6414–6432.
https://doi.org/10.1029/2019JC015111
Hu, X., Sun, J., Chan, T. O., & Myers, P. G. (2018). Thermodynamic and
dynamic ice thickness contributions in the Canadian Arctic Archipelago
in NEMO-LIM2 numerical simulations. The Cryosphere, 12(4), 1233–
1247. https://doi.org/10.5194/tc-12-1233-2018
Hughes, H. G., Klymak, J. M., Hu, X., & Myers, P. G. (2017). Water mass
modiﬁcation and mixing rates in a 1/12° simulation of the Canadian
Arctic Archipelago. Journal of Geophysical Research: Oceans, 122(2),
803–820. https://doi.org/10.1002/2016JC012235
Hunke, E. C., & Dukowicz, J. K. (1997). An elastic-viscous-plastic model for
sea ice dynamics. Journal of Physical Oceanography, 27(9), 1849–1867.
https://doi.org/10.1175/1520-0485(1997)027<1849:AEVPMF>2.0.CO;2
Lincoln, B. J., Rippeth, T. P., Lenn, Y.-D., Timmermans, M. L., Williams, W.
J., & Bacon, S. (2016). Wind-driven mixing at intermediate depths in an
ice-free Arctic Ocean. Geophysical Research Letters, 43(18), 9749–
9756. https://doi.org/10.1002/2016GL070454
Madec, G., & the NEMO team. (2008). NEMO ocean engine (Notes du póle
de modélisation de No. 27). d’Institut Pierre-Simon Laplace (IPSL).
Masina, S., Storto, A., Ferry, N., Valdivieso, M., Haines, K., Balmaseda, M.,
Zuo, H., Drevillon, M., & Parent, L. (2017). An ensemble of eddy-permit-
ting global ocean reanalyses from the MyOcean project. Climate
Dynamics, 49(3), 813–841. https://doi.org/10.1007/s00382-015-2728-5
McLaughlin, F. A., Carmack, E. C., Ingram, R. G., Williams, W. J., &
Michel, C. (2004). Oceanography of the Northwest Passage. In A. R.
Robinson & K. H. Brink (Eds.), The sea (Vol. 14, pp. 1211–1242).
Harvard University Press.
McPhee, M. G., Proshutinsky, A., Morison, J. H., Steele, M., & Alkire, M. B.
(2009). Rapid change in freshwater content of the Arctic Ocean.
Geophysical Research Letters, 36(10), L10602. https://doi.org/10.1029/
2009GL037525
Michel, C., Hamilton, J., Hansen, E., Barber, D., Reistad, M., Iacozza, J.,
Seuthe, L., & Niemi, A. (2015). Arctic Ocean outﬂow shelves in the chan-
ging Arctic: A review and perspectives. Progress in Oceanography, 139,
66–88. https://doi.org/10.1016/j.pocean.2015.08.007
Myers, P. G., & Ribergaard, M. H. (2013). Warming of the polar water layer
in Disko Bay and potential impact on Jakobshavn Isbrae. Journal of
Physical Oceanography, 43(12), 2629–2640. https://doi.org/10.1175/
JPO-D-12-051.1
Nurser, A., & Bacon, S. (2014). The Rossby radius in the Arctic Ocean.
Ocean Science, 10(6), 967–975. https://doi.org/10.5194/os-10-967-2014
Padman, L., & Erofeeva, S. (2004). A barotropic inverse tidal model for the
Arctic Ocean. Geophysical Research Letters, 31(2), L02303. https://doi.
org/10.1029/2003GL019003
Proshutinsky, A., Krishﬁeld, R., Toole, J. M., Timmermans, M., Williams,
W., Zimmermann, S., Yamamoto-Kawai, M., Armitage, T. W. K.,
Dukhovskoy, D., Golubeva, E., Manucharyan, G. E., Platov, G.,
Watanabe, E., Kikuchi, T., Nishino, S., Itoh, M., Kang, S.-H., Cho, K.-
H., Tateyama, K., & Zhao, J. (2019). Analysis of the Beaufort Gyre fresh-
water content in 2003–2018. Journal of Geophysical Research: Oceans,
124(12), 9658–9689. https://doi.org/10.1029/2019JC015281
Rainville, L., Lee, C. M., & Woodgate, R. A. (2011). Impact of wind-driven
mixing in the Arctic Ocean. Oceanography, 24(3), 136–145. https://doi.
org/10.5670/oceanog.2011.65
Shimada, K., Kamoshida,
T., Itoh, M., Nishino, S., Carmack, E.,
McLaughlin, F., Zimmermann, S., & Proshutinsky, A. (2006). Paciﬁc
Effects of Seasonal Ice Coverage on the Kitikmeot Sea / 231
ATMOSPHERE-OCEAN 59 (4–5) 2021, 214–232
https://doi.org/10.1080/07055900.2021.1965531
Canadian Meteorological and Oceanographic Society

Ocean inﬂow: Inﬂuence on catastrophic reduction of sea ice cover in the
Arctic Ocean. Geophysical Research Letters, 33, L08605. https://doi.org/
10.1029/2005GL025624.
Smith, G. C., Roy, F., Mann, P., Dupont, F., Brasnett, B., Lemieux, J.-F.,
Laroche, S., & Bélair, S. (2014). A new atmospheric dataset for forcing
ice–ocean models: Evaluation of reforecasts using the Canadian global
deterministic
prediction
system.
Quarterly
Journal
of
the
Royal
Meteorological Society, 140(680), 881–894. https://doi.org/10.1002/qj.2194
Smith, W. H., & Sandwell, D. T. (1997). Global sea ﬂoor topography from
satellite altimetry and ship depth soundings. Science, 277(5334), 1956–
1962. https://doi.org/10.1126/science.277.5334.1956
Timco, G. W., & Frederking, R. M. W. (1996). A review of sea ice density.
Cold Regions Science and Technology, 24(1), 1–6. https://doi.org/10.1016/
0165-232X(95)00007-X
Williams, W., Brown, K., Bluhm, B., Carmack, E., Dalman, L.,
Danielson, S., Else, B., Fredriksen, R., Mundy, C., Rotermund, L., &
Schimnowski,
A.
(2018).
Stratiﬁcation
in
the
Canadian
Arctic
Archipelago’s Kitikmeot Sea: Biological and geochemical conse-
quences. In 2018 Polar Knowledge: Aqhaliat report (pp. 46–52).
Polar Knowledge Canada.
Woodgate, R. A. (2018). Increases in the Paciﬁc inﬂow to the Arctic from
1990 to 2015, and insights into seasonal trends and driving mechanisms
from year-round Bering Strait mooring data. Progress in Oceanography,
160, 124–154. https://doi.org/10.1016/j.pocean.2017.12.007
Woodgate, R. A., Aagaard, K., & Weingartner, T. J. (2006). Interannual
changes in the Bering Strait ﬂuxes of volume, heat and freshwater
between 1991 and 2004. Geophysical Research Letters, 33(15), L15609.
https://doi.org/10.1029/2006GL026931
Woodgate, R. A., Weingartner, T. J., & Lindsay, R. W. (2010). The 2007
Bering Strait oceanic heat ﬂux and anomalous Arctic sea-ice retreat.
Geophysical Research Letters, 37(1), L01602. https://doi.org/10.1029/
2009GL041621
232 / Chengzhu Xu et al.
ATMOSPHERE-OCEAN 59 (4–5) 2021, 214–232
https://doi.org/10.1080/07055900.2021.1965531
La Société canadienne de météorologie et d’océanographie