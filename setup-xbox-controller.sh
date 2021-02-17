#!/bin/bash
###############################################################
#                Unofficial 'Bash strict mode'                #
# http://redsymbol.net/articles/unofficial-bash-strict-mode/  #
###############################################################
set -euo pipefail
IFS=$'\n\t'
###############################################################


###############################################################
### Install xbox controller driver ###
###############################################################
echo "Installing xbox controller..."
sudo apt-get install xboxdrv
echo ""


###############################################################
### Disable Enhanced Re-Transmission Mode (ERTM) ###
###############################################################
echo "Disabling Enhanced Re-Transmission Mode..."
echo 'options bluetooth disable_ertm=Y' | sudo tee -a /etc/modprobe.d/bluetooth.conf
echo "Disabling Enhanced Re-Transmission Mode, Please Rboot the pi-top"


