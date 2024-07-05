import numpy as np

f = [100, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 200]
alpha = 0.5  # np.random.rand()

delta = alpha * np.mean(f)
sum = f[0]
i = 0

individuals = []

while True:
    if (delta < sum):
        individuals.append(f[i])
        delta += sum
    else:
        i += 1
        if (i >= len(f)):
            break
        sum += f[i]

    if (i >= len(f)):
        break


print(individuals)
