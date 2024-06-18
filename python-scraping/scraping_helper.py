from bs4 import BeautifulSoup
import requests
import re
from collections import defaultdict

all_shoes = list()

buzz_url = 'https://www.buzzsneakers.ba'

buzz_products_div = 'item-data col-xs-12 col-sm-12'

# check pattern for href attributes for a tag


def check_pattern_for_link(url):
    pattern = r'^https://www.buzzsneakers.ba/patike/.+$'
    return re.match(pattern, url) is not None

# find link for shoe


def find_product_url(tag):
    set_of_links = set()
    for children in tag.find_all('a'):
        url = children.get('href')
        if check_pattern_for_link(url):
            set_of_links.add(url)

    if len(set_of_links) == 1:
        return set_of_links.pop()
    raise Exception("Can't filter url!")

# method accepts parrent tag with data and returns image url for single shoe


def find_photo_url(tag):
    for children in tag.find_all('img'):
        src = children.get('src')
        if 'slike-proizvoda' in src and src is not None:
            return src
        raise Exception('Can\'t find image')

# method accepts parrent tag for single shoe and returns price


def find_product_price(tag):
    price_tag = tag.find('div', class_='current-price')
    return price_tag.get_text(strip=True)


r = requests.get(
    "https://www.buzzsneakers.ba/patike/za-muskarce+unisex/za-odrasle")

class_name = 'wrapper-grid-view item product-item ease col-xs-6 col-sm-4 col-md-3 col-lg-3 grid-view'
soup = BeautifulSoup(r.content, 'html.parser')


shoes_list = list()
for div_tag in soup.find_all('div', class_=buzz_products_div):
    shoes_list.append(div_tag)


for shoe_div in shoes_list:
    url = find_product_url(shoe_div)
    photo_src = buzz_url + find_photo_url(shoe_div)
    price = find_product_price(shoe_div)
    all_shoes.append({
        "url": url,
        "photo_url": photo_src,
        "price": price,
        "category": 'shoes'
    })
