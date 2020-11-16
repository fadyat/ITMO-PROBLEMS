import math

# to see my image
from matplotlib import pyplot as plt
# to download, show, save my image
from skimage.io import imread, imshow, imsave
# my image
img = imread('kek_black.jpg')
# show me my image
plt.imshow(img)
plt.show()

# pixels from 64 line
p = []

for i in range(63, 64):
    for j in range(0, 128):
        p.append(img[i][j])

print('\n', "Before: ")

for i in range(0, len(p)):
    print("%4d" % p[i], end=' ')
    if i % 12 == 11:
        print()

# here we will write the probably 0...240
total = []
for i in (range(0, 256 // 20 + 1)):
    total.append(0)

print('\n\n', "After: ")

for i in range(0, len(p)):
    p[i] = round(p[i] // 20) * 20
    print("%4d" % p[i], end=' ')
    total[p[i] // 20] += 1
    if i % 12 == 11:
        print()

print()

# here we will save count of numbers and round pixel
pairs = []
for i in range(0, len(total)):
    pairs.append((total[i], 20 * i))

print('\n', '|', end="")
# sort
pairs.sort(reverse=True)
# print
for i in range(0, len(pairs)):
    print("%4d" % pairs[i][1], '|', end=' ')
print('\n', '|', end="")

for i in range(0, len(pairs)):
    print("%4d" % pairs[i][0], '|', end=' ')
print('\n', '|', end="")

for i in range(0, len(pairs)):
    print("%4.2f" % float(pairs[i][0] / 128), '|', end=' ')
print()

# ans - my entropy = -∑(i...n) = p[i]*log2(p[i])
ans = 0.00
# import math in the top of the code
for i in range(0, len(pairs)):
    if pairs[i][0] != 0:
        ans -= float(pairs[i][0] / 128) * (math.log2(float(pairs[i][0] / 128)))
print('\n', "%.4f" % ans)

print()
# CONST LEN
# bu has binary codes for numbers
bu = ['----', '1001', '0111', '1000', '----', '----', '0110', '0101', '0011', '0000', '0001', '0100', '0010']
for i in range(0, len(p)):
    print("%5s" % bu[p[i] // 20], end=' ')
    if i % 12 == 11:
        print()
print('\n')

# CODE SHEN-FANO
fano = ['----', '000000', '00001', '000001', '----', '----', '00010', '00011', '0010', '11', '10', '0011', '01']
for i in range(0, len(p)):
    print("%7s" % fano[p[i] // 20], end=' ')
    if i % 12 == 11:
        print()
print('\n')

# CODE HAFF
haf = ['----', '000000', '00001', '000001', '----', '----', '00010', '00011', '0011', '11', '10', '0010', '01']
for i in range(0, len(p)):
    print("%8s" % haf[p[i] // 20], end=' ')
    if i % 12 == 11:
        print()
print()

