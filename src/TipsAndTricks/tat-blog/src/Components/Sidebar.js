import React from "react";
import SearchForm from "./SearchForm";
import CategoriesWidget from "./Widgets/CategoriesWidget";
import FeaturedPostsWidget from "./Widgets/FeaturedPostsWidget";
import RandomPostsWidget from "./Widgets/RandomPostsWidget";
import TagCloudWidget from "./Widgets/TagCloudWidget";
import BestAuthorsWidget from "./Widgets/BestAuthorsWidget";
import ArchivesWidget from "./Widgets/ArchivesWidget";
import NewsletterForm from "./NewsletterForm";

const Sidebar = () => {
    return (
      <div className="pt-4 ps-2">
        <SearchForm />
        <CategoriesWidget />
        <FeaturedPostsWidget />
        <RandomPostsWidget />
        <TagCloudWidget />
        <BestAuthorsWidget />
        <ArchivesWidget />
        <NewsletterForm />
      </div>  
    )
}

export default Sidebar;