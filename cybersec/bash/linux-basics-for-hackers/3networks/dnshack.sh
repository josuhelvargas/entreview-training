#bin/bash

#DNS Hacking
# There are several ways that a malicious actor may attempt to hack or exploit a DNS system:

# DNS Spoofing: 
# This is a type of attack in which an attacker intercepts and alters DNS traffic, 
# redirecting users to a different website than the one they intended to visit. 
# This can be done through a variety of methods, such as ARP spoofing or by compromising a DNS server.

# DNS Cache Poisoning: 
# This is a type of attack in which an attacker corrupts the DNS cache on a DNS server,
# causing it to return incorrect IP addresses. This can allow the attacker to redirect users to a malicious website.

# DNS Amplification Attack:
# This is a type of Distributed Denial of Service (DDoS) attack in which an attacker spoofs the source IP address of a DNS query,
#  causing the targeted DNS server to send a large amount of traffic to the victim.

# DNS Hijacking: 
# This is a type of attack in which an attacker takes control of a domain name by modifying the DNS records associated with it,
# redirecting traffic intended for the legitimate website to a different website under the attacker's control.

# Phishing: 
# this is a social engineering attack where an attacker sends an email or a link to a website that looks like a legitimate one, but once the user click on it and enters their credentials, the attacker will have access to the user's data.

# It's important to note that these are just a few examples and new techniques for attacking DNS systems are constantly being developed.
# It's also important to keep your DNS and other network infrastructure up to date 
# with the latest security patches and to implement best practices for securing DNS servers, such as using DNSSEC.

echo "Welcome to DNS Hacking!"



ifconfig 
iwconfig


#cambiar ipadress 
ifconfig <interface> <ip value>

#cambiar network mask y broadcast address
ifconfig <interfafce> <ip value> netmas <value>  broadcast <value>


#spoof macaddress de eternet
ifconfig <interface> down
ifconfig <interface> hw ether <mac address> 
ifconfig <interface> up 




#punto importante para hackers conocer dhcp ( ya que debees tener una direccionasignada si estas en una red) 
dhclient <interface> 


dig <domain> #get dns info


#how to change dnsserver :
leafpad /etc/resolv.conf

#nameserver? 8.8.8.8 como funciona y que hace nameserver ? 

leafpad /etc/hosts



# 5️⃣ Reset IP to DHCP-assigned address
# If using dhclient:
# sudo dhclient -r eth0
# sudo dhclient eth0
# If using systemd-networkd:
# sudo systemctl restart systemd-networkd
# If using NetworkManager:
# sudo systemctl restart NetworkManager
nslookup example.com





2806:2a0:2f::2
2806:2a0:25::2

#Eso pertenece a un ISP mexicano (probablemente Telmex/Infinitum u otro ISP).

Seguridad:

#Tu tráfico DNS está pasando por tu proveedor.

#Pueden:

#Loggear consultas

#Filtrar dominios

#Aplicar DNS hijacking

#Inyectar NXDOMAIN redirects