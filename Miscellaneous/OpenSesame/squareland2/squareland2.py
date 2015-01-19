# Import the required modules.
import os
import subprocess
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
		
		win.winHandle.minimize() #minimize the PsychoPy window
		win.fullscr = False #disable fullscreen
		win.flip() #redraw the (minimized) window

		try:
			os.chdir(self.get('path'))
			subprocess.call([self.get('path') + "\\SQUARELAND2.0.exe"]) #launch external program
			#print self.get('path') + "\\SQUARELAND2.0.exe"
		except WindowsError:
			print "SQUARELAND2.0 application couldn't be launched!"
			
		win.winHandle.maximize() #when external program closes, maximize PsychoPy window again
		win.fullscr = True 
		win.winHandle.activate() #re-activate window
		win.flip() #redraw the newly activated window
		

class qtsquareland2(squareland2, qtautoplugin):

	def __init__(self, name, experiment, script=None):

		# Call parent constructors.
		squareland2.__init__(self, name, experiment, script)
		qtautoplugin.__init__(self, __file__)