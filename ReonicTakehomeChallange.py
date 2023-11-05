#number of parking spaces 200
#number of charging stations 20
numberOfChargeStations = 20
#charging speed 11kW
chargingSpeed = 11
#number of ticksPerYear 35040
ticksPerYear = 35040
#number of ticksPerDay 96
ticksPerDay = 96
#number of ticksPerHour 4
ticksPerHour = 4
#number of daysPerYear 365

import random
import math
import numpy as np
import matplotlib.pyplot as plt
import matplotlib.animation as animation
from matplotlib import style
import matplotlib.patches as patches
import matplotlib.path as path



#probability of arrival for each hour of the day
arivalProb = [0.94, 0.94, 0.94, 0.94, 0.94, 0.94, 0.94, 0.94, 2.83, 2.83, 5.66, 5.66, 5.66,
              7.55, 7.55, 7.55, 10.38, 4.72, 4.72, 4.72, 0.94, 0.94]
#probability distribution of an arriving EV's charging needs
chargingNeedsProp = [34.31, 4.90, 9.80, 11.76, 8.82, 11.76, 10.78, 4.90, 2.94]

# kWh per 100km for each EV
kWhPer100km = 18
#caclulate the total energy consumed in kWh
def calculateEnergyConsumed():
    

def the