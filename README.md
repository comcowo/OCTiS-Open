# OCTiS-Open
Open Source Software by OCTiS GmbH

OCTiS.Knx
=========

The OCTiS.Knx project is intended to load the knxproj export file of the ETS application and transfer the entities into a Home Assistant configuration file.
Click 'Open' at the OCTiS.Knx.HomeAutomationConfigurator to load knxproj.
After selecting at least one group address entity at the grid, a line in the Home Assistant configuration content can be created by clicking one of the buttons in th middle.
It should be compatible to ETS version > 4.0. But I tried it only with ETS > 6.0.
Password protected knxproj files are not supported.

ETS: https://www.knx.org/
Home Assistant: https://www.home-assistant.io/

Many thanks to https://github.com/danielwerthen/Funcis-Sharp, where I get the Knx-Model code from.
