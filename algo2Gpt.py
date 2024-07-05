import numpy as np


f = [100, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 200]
fTotal = np.sum(f)
popSize = len(f)
numSelection = 10
P = fTotal / numSelection

selected = []

start_point = 0.1 * P  # np.random.rand() * P
current_point = start_point

for selection_point in range(numSelection):
    sum_fitness = 0
    for indivFit in f:
        sum_fitness += indivFit
        if sum_fitness > current_point:
            selected.append(indivFit)
            current_point += P

print(selected)
