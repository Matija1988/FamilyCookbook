import { useEffect, useState } from "react";
import useLoading from "../../../hooks/useLoading";
import TagsService from "../../../services/TagsService";
import PaginationForTags from "./PaginationForTags";

export default function TagList() {
  const [tags, setTags] = useState([]);
  const [pageNumber, setPageNumber] = useState(1);
  const [pageSize, setPageSize] = useState(10);
  const [totalCount, setTotalCount] = useState(0);
  const [pageCount, setPageCount] = useState(0);
  const { showLoading, hideLoading } = useLoading();

  const getRequestParams = (pageSize, pageNumber) => {
    let params = {};
    if (pageSize) {
      params["PageSize"] = pageSize;
    }
    if (pageNumber) {
      params["PageNumber"] = pageNumber;
    }
    return params;
  };

  const handlePageChange = (value) => {
    setPageNumber(value);
  };
  async function paginateTags() {
    showLoading();
    const params = getRequestParams(pageSize, pageNumber);
    const response = await TagsService.paginate("tag/tags", params);

    if (response.ok) {
      setTags(response.data.items);
      setTotalCount(response.data.totalCount);
      setPageCount(response.data.pageCount);
      hideLoading();
    }
    hideLoading();
  }

  useEffect(() => {
    paginateTags();
  }, [pageNumber, pageSize]);

  return (
    <>
      <div className="tag-list-container">
        <h4>Available tags</h4>
        <div className="tag-list-group">
          {tags ? (
            tags.map((tag) => (
              <div key={tag.id} className="tag-list-group-item">
                {tag.text}
              </div>
            ))
          ) : (
            <div>
              <p>No tags found!</p>
            </div>
          )}
        </div>
        <div className="pagination-container">
          <PaginationForTags
            pageNumber={pageNumber}
            totalPages={pageCount}
            handlePageChange={handlePageChange}
          ></PaginationForTags>
        </div>
      </div>
    </>
  );
}
