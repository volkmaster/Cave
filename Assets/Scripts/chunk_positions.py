import sys
from math import sqrt
import pyperclip

if __name__ == '__main__':
	radius = int(sys.argv[1])

	data = []
	for i in range(-radius, radius + 1):
		for j in range(-radius, radius + 1):
			data.append([i, j, sqrt(i**2 + j**2)])
	data.sort(key=lambda x: x[2])

	text = ''
	for i, item in enumerate(data):
		end = ', '
		if i == len(data) - 1:
			end = '\n'
		elif (i + 1) % 5 == 0 and i != 0:
			end = ',\n'
		content = 'new WorldPosition(%d, 0, %d)' % (item[0], item[1])
		text += content + end
		print(content, end=end)
	pyperclip.copy(text)
