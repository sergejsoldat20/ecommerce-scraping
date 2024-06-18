from bs4 import BeautifulSoup
import requests
import re
from collections import defaultdict


def get_data():
    try:
        # response = requests.get(
        #     "https://www.juventasport.com/getItems?category_id=9537&sex_id=9779&sex_id[]=8983&limit=1000")
        response = requests.get(
            "https://www.juventasport.com/getItems?category_id=9537&sex_id=9779&sex_id[]=8983&limit=1000")
        if response.status_code == 200:
            # Assuming the response is in JSON format
            data = response.json()

            # Count the elements in the response (e.g., the number of items in a list)
            element_count = len(data["products"]["data"])

            return element_count
    except Exception as e:
        print("fuck")


print(get_data())
