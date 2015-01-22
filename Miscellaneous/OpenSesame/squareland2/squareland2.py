# Import the required modules.
import shutil
import subprocess
from os import chdir
from os import listdir
from os.path import isfile, join

import pygame
import psychopy
from libopensesame import debug
from libopensesame.item import item
from libqtopensesame.items.qtautoplugin import qtautoplugin

class squareland2(item):

	description = u'Plug-in description'

	def __init__(self, name, experiment, script=None):

		"""
		Constructor.

		Arguments:
		name		--	The name of the plug-in.
		experiment	--	The experiment object.

		Keyword arguments:
		script		--	A definition script. (default=None)
		"""
		
		# Call the parent constructor.
		item.__init__(self, name, experiment, script)

	def prepare(self):

		# Call parent functions.
		item.prepare(self)
		# Prepare your plug-in here.

		#content = ""
		#with open(self.experiment.pool_folder + '\\' + self.get('settings_file'), 'r') as content_file:
		#	content = content_file.read()
		#	
		#target = open("plugins\\squareland2\\SQUARELAND2.0\\Assets\\Settings.xml", 'w')
		#target.truncate()
		#target.write(content)
		#target.close()
		#	
		#content = ""
		#with open(self.experiment.pool_folder + '\\' + self.get('procedure_file'), 'r') as content_file:
		#	content = content_file.read()
		#
		#target = open("plugins\\squareland2\\SQUARELAND2.0\\Assets\\Procedure.xml", 'w')
		#target.truncate()
		#target.write(content)
		#target.close()
		
		
	def run(self):

		# Record the timestamp of the plug-in execution.
		self.set_item_onset()
		# Run your plug-in here.
		
		win = self.experiment.window
		
		if self.experiment.canvas_backend == 'psycho':
			win.winHandle.minimize()
			win.fullscr = False 
			win.flip()
		else: 
			pygame.display.iconify()
		
		if True == True:
			try:
				chdir(self.get('path'))
				subprocess.call([self.get('path') + "\\SQUARELAND2.0.exe"]) #launch external program
				#print self.get('path') + "\\SQUARELAND2.0.exe"
			
				path = self.get('path') + "\\LogFiles\\"
			
				for f in listdir(path):
					f_split = f.split('.')
					file = join(path, f)
					if isfile(file) and f_split.pop() == "txt":
						dst = self.get('path') + "\\..\\%s_" % self.get('subject_nr')
						if isfile(dst + f) == True:
							count = 1
							dst2 = dst + '%s_' % count
							while isfile(dst2 + f) == True:
								count += 1
								dst2 = dst + '%s_' % count
							dst = dst2
						shutil.copyfile(file, dst + f)
			except WindowsError:
				print "SQUARELAND2.0 application couldn't be launched!"
		
		if self.experiment.canvas_backend == 'psycho':
			win.winHandle.activate()
			win.winHandle.maximize()
			win.fullscr = True 
			win.flip()
		else:
			pygame.display.set_mode()
		

class qtsquareland2(squareland2, qtautoplugin):

	def __init__(self, name, experiment, script=None):

		# Call parent constructors.
		squareland2.__init__(self, name, experiment, script)
		qtautoplugin.__init__(self, __file__)