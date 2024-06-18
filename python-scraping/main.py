from bs4 import BeautifulSoup
import requests


r = requests.get(
    "https://www.buzzsneakers.ba/patike/za-muskarce+unisex/za-odrasle")


soup = BeautifulSoup(r.content, 'html.parser')

class_list = sorted(['wrapper-grid-view', 'item', 'product-item', 'ease',
                     'col-xs-6', 'col-sm-4', 'col-md-3', 'col-lg-3', 'grid-view'])
check_set = {'wrapper-grid-view', 'item', 'product-item'}
result_list = list()
for tag in soup.find_all('div'):
    if tag.get('class') is not None:
        if sorted(tag.get('class')) == class_list:
            result_list.append(tag)


children_list = result_list[1].find_all(recursive=True)

print(len(children_list))
